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

    public double[] levelWeight = new double[3];

    public float[] levelLine = new float[3];

    [SerializeField]
    private int level;
    [SerializeField]
    private int currentLevel;

    [SerializeField]
    float lineSpeed = 2f;
    float appliedLineSpeed = 2f;
    bool isBoosting = false;

    public Action<float> lineSpeedAction;

    public float LineSpeed
    {
        get { return lineSpeed; }
        set
        {
            lineSpeed = value;
            if (isBoosting)
                return;
            if (lineSpeedAction != null)
                lineSpeedAction.Invoke(lineSpeed);
            appliedLineSpeed = lineSpeed;
            foreach (GameObject go in lineQueue)
            {
                go.GetComponent<LinesMove>().speed = appliedLineSpeed;
            }
        }
    }

    void Start()
    {
        StartPosition = new Vector3(0, -10, 0);
        EndPosition = new Vector3(0, 10, 0);
        LineSpeed = Managers.Data.GetSkin(GameManagerEx.Instance.player.MonkeySkinId).Speed;
        SetDictionary();
        SetWrPick(0);
        GameManagerEx.Instance.distance.distanceEvent -= AddWrPick;
        GameManagerEx.Instance.distance.distanceEvent += AddWrPick;
        GameManagerEx.Instance.distance.distanceEvent -= LineSpeedUp;
        GameManagerEx.Instance.distance.distanceEvent += LineSpeedUp;
    }




    void SetDictionary()
    {
        string maplevel = string.Empty;
        GameObject[] maps = null;
        for (int i = 0; i <= maxLineLv; i++)
        {
            maplevel = $"level {i}";
            maps = Resources.LoadAll<GameObject>($"Prefabs/Map/{GameManagerEx.Instance.mapID}/{maplevel}");
            levelLinesDict.Add(i, maps);
        }
        maplevel = $"level Event";
        maps = Resources.LoadAll<GameObject>($"Prefabs/Map/{GameManagerEx.Instance.mapID}/{maplevel}");
        levelLinesDict.Add(maxLineLv + 1, maps);

        foreach (KeyValuePair<int, GameObject[]> keyValuePair in levelLinesDict)
        {
            int i = keyValuePair.Key;
            string name = string.Empty;
            foreach (GameObject go in keyValuePair.Value)
            {
                name += go.name + ", ";
            }
            Debug.Log($"level {i} list - {name}");
        }
    }


    void Update()
    {
        if (distance >= 10.0f)
        {
            NewLines = MakeLinesPlay();
            // Debug.Log("create success");
            NewLines.transform.position = StartPosition;
            lineQueue.Enqueue(NewLines);
            distance = 0;
        }
        distance = NewLines.transform.position.y - StartPosition.y;

        CheckPickNum();
    }

    void SetWrPick(int lv)
    {
        wrPicker = new Rito.WeightedRandomPicker<int>();
        for (int i = 0; i <= maxLineLv; i++)
        {
            Debug.Log($"{level}");
            if (i == lv)
            {
                wrPicker.Add(i, 10);
                level = i;
                if (i == 0)
                    currentLevel = level;
                else
                {
                    currentLevel = maxLineLv + 1;
                }
            }
            else
                wrPicker.Add(i, 0);
        }
    }

    private void CheckPickNum()
    {
        for (int i = 0; i < levelWeight.Length; i++)
        {
            levelWeight[i] = wrPicker.GetWeight(i + 1);
        }
    }

    GameObject MakeLinesPlay()
    {
        GameObject mNewLines = null;

        while (true)
        {
            int level = settingLevel();

            int lineNum = SettingLineNum(level);

            if (curLineName.Equals(string.Empty) || curLineName.Equals(levelLinesDict[level][lineNum].name))
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
        if (level != currentLevel)
        {
            int lv = currentLevel;
            currentLevel = level;
            return lv;
        }

        return currentLevel;
    }

    private void AddWrPick(int value)
    {
        if (levelLine[level] < value)
        {
            Debug.Log($"{level} {levelLine[level]}, {value}");
            SetWrPick(level + 1);
        }
    }

    private void LineSpeedUp(int dist)
    {
        if (dist > 0 && dist % 100 == 0)
        {
            GameManagerEx.Instance.makeLines.LineSpeed += 0.3f;
        }
    }

    Coroutine boostLineSpeedCoroutine;
    public void BoostLineSpeed(float time, float force)
    {
        if (boostLineSpeedCoroutine != null)
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
