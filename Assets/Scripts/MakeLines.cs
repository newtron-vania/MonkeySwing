using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLines : MonoBehaviour
{
    [SerializeField]
    Dictionary<int, List<GameObject>> levelLinesDict = new Dictionary<int, List<GameObject>>();

    float distance = 11.0f;
    private Vector3 StartPosition;
    private Vector3 EndPosition;

    GameObject NewLines = null;

    public Queue<GameObject> lineQueue = new Queue<GameObject>();

    [SerializeField]
    float lineSpeed = 2f;
    float appliedLineSpeed = 2f;

    public float LineSpeed { 
        get { return lineSpeed; } 
        set { 
            lineSpeed = value;
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
    }
    // Update is called once per frame
    void Update()
    {
        if(distance >= 10.0f){
            NewLines = MakeLinesPlay();
            //Debug.Log("create success");
            NewLines.transform.position = StartPosition;
            lineQueue.Enqueue(NewLines);
            distance = 0;
        }
        distance = NewLines.transform.position.y - StartPosition.y;

    }

    GameObject MakeLinesPlay(){
        int level = settingLevel();


        int lineNum = SettingLineNum(level);
        GameObject line = levelLinesDict[level][lineNum];


        GameObject mNewLines = null;

        mNewLines = Managers.Resource.Instantiate(line, transform.position);

        LinesMove linesMove = mNewLines.GetComponent<LinesMove>();
        
        linesMove.speed = appliedLineSpeed;
        return mNewLines;
    }

    int SettingLineNum(int level)
    {
        //각 라인의 생성유무(pool을 확인)하여 제외하고 다시 반복
        int num = Random.Range(0, levelLinesDict[level].Count);
        return num;
    }

    int settingLevel()
    {
        //각 레벨별 가중치 처리 필요
        int level = Random.Range(1, levelLinesDict.Count+1);

        return level;
    }

    public void BoostLineSpeed(float time, float force)
    {
        StopCoroutine("BoostLineSpeedCoroutine");
        StartCoroutine(BoostLineSpeedCoroutine(time, force));
    }

    IEnumerator BoostLineSpeedCoroutine(float time, float force)
    {
        appliedLineSpeed = lineSpeed;
        appliedLineSpeed *= force;
        foreach (GameObject go in lineQueue)
        {
            go.GetComponent<LinesMove>().speed = appliedLineSpeed;
        }
        yield return new WaitForSeconds(time);
        appliedLineSpeed = lineSpeed;
        foreach (GameObject go in lineQueue)
        {
            go.GetComponent<LinesMove>().speed = appliedLineSpeed;
        }
    }
}
