using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MakeLines : MonoBehaviour
{
    [SerializeField]
    Dictionary<int, GameObject[]> levelLinesDict = new Dictionary<int, GameObject[]>();
    [SerializeField]
    int maxLineLv = 3;

    float distance = 11.0f;
    private Vector3 StartPosition;
    private Vector3 EndPosition;

    GameObject NewLines = null;

    public Queue<GameObject> lineQueue = new Queue<GameObject>();
    string curLineName = string.Empty;

    Rito.WeightedRandomPicker<int> wrPicker;
    [SerializeField]
    private float wNum = 30;
    private int createCount = 0;

    [SerializeField]
    private int MaxCreateCount = 8;

    [SerializeField]
    float lineSpeed = 2f;
    float appliedLineSpeed = 2f;
    bool isBoosting = false;

    public Action<float> lineSpeedAction;

    public float LineSpeed { 
        get { return lineSpeed; } 
        set { 
            lineSpeed = value;
            if (isBoosting)
                return;
            if(lineSpeedAction != null)
                lineSpeedAction.Invoke(lineSpeed);
            appliedLineSpeed = lineSpeed;
            foreach (GameObject go in lineQueue)
            {
                go.GetComponent<LinesMove>().speed = appliedLineSpeed;
            }
        } 
    }

    void Start() {
        StartPosition = new Vector3(0,-10,0);
        EndPosition = new Vector3(0,10,0);
        appliedLineSpeed = lineSpeed;
        SetDictionary();
        SetWrPick();
        GameManagerEx.Instance.distance.distanceEvent -= AddWrPick;
        GameManagerEx.Instance.distance.distanceEvent += AddWrPick;
    }


    void SetWrPick()
    {
        float w = wNum;
        wrPicker = new Rito.WeightedRandomPicker<int>();
        for (int i = 1; i <= maxLineLv; i++)
        {
            wrPicker.Add(i, w);
            w /= 10;
        }
    }

    void SetDictionary()
    {
        string maplevel = string.Empty;
        GameObject[] maps = null;
        for (int i=1; i<=maxLineLv; i++)
        {
            maplevel = $"level {i}";
            maps = Resources.LoadAll<GameObject>($"Prefabs/Map/{maplevel}");
            levelLinesDict.Add(i, maps);
        }
        maplevel = $"level Event";
        maps = Resources.LoadAll<GameObject>($"Prefabs/Map/{maplevel}");
        levelLinesDict.Add(maxLineLv+1, maps);

        foreach(KeyValuePair<int, GameObject[]> keyValuePair in levelLinesDict)
        {
            int i = keyValuePair.Key;
            string name = string.Empty;
            foreach (GameObject go in keyValuePair.Value)
            {
                name += go.name+", ";
            }
            Debug.Log($"level {i} list - {name}");
        }
    }


    void Update()
    {
        if(distance >= 10.0f){
            NewLines = MakeLinesPlay();
            // Debug.Log("create success");
            NewLines.transform.position = StartPosition;
            lineQueue.Enqueue(NewLines);
            distance = 0;
        }
        distance = NewLines.transform.position.y - StartPosition.y;
    }

    GameObject MakeLinesPlay(){
        GameObject mNewLines = null;

        while (true)
        {
            int level = settingLevel();

            int lineNum = SettingLineNum(level);

            if(curLineName.Equals(string.Empty) || curLineName.Equals(levelLinesDict[level][lineNum].name))
            {
                mNewLines = Managers.Resource.Instantiate(levelLinesDict[level][lineNum], transform.position);
                break;
            }
        }

        LinesMove linesMove = mNewLines.GetComponent<LinesMove>();
        
        linesMove.speed = appliedLineSpeed;
        return mNewLines;
    }

    private int SettingLineNum(int level)
    {
        //각 라인의 생성유무(pool을 확인)하여 제외하고 다시 반복
        int num = UnityEngine.Random.Range(0, levelLinesDict[level].Length);
        return num;
    }

    private int settingLevel()
    {
        int level = 0;
        if (createCount < MaxCreateCount)
        {
            //각 레벨별 가중치 처리 필요
            level = wrPicker.GetRandomPick();
            createCount += 1;
        }
        else
        {
            level = maxLineLv + 1;
            createCount = 0;
        }
        return level;
    }

    private void AddWrPick(int value)
    {
        if(value % 50 == 0)
        for (int i = 1; i <= maxLineLv; i++)
        {
            double w = wrPicker.GetWeight(i);
            if (wrPicker.GetWeight(i) < wNum*i)
            {
                    wrPicker.ModifyWeight(i, w + wrPicker.GetWeight(i-1) *0.2);
            }z
                Debug.Log($"weight {i} : {wrPicker.GetWeight(i)}");
        }
    }

    Coroutine boostLineSpeedCoroutine;
    public void BoostLineSpeed(float time, float force)
    {
        if(boostLineSpeedCoroutine != null)
            StopCoroutine(boostLineSpeedCoroutine);
        boostLineSpeedCoroutine = StartCoroutine(BoostLineSpeedCoroutine(time, force));
    }

    IEnumerator BoostLineSpeedCoroutine(float time, float force)
    {
        isBoosting = true;
        appliedLineSpeed = lineSpeed * force;
        foreach (GameObject go in lineQueue)
        {
            go.GetComponent<LinesMove>().speed = appliedLineSpeed;
        }
        yield return new WaitForSeconds(time);
        isBoosting = false;
        appliedLineSpeed = lineSpeed;
        foreach (GameObject go in lineQueue)
        {
            go.GetComponent<LinesMove>().speed = appliedLineSpeed;
        }
    }
}
