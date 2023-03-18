using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLines : MonoBehaviour
{
    [SerializeField]
    GameObject lev1, lev2;

    int LinesObj;
    float distance = 11.0f;
    private Vector3 StartPosition;
    private Vector3 EndPosition;

    GameObject NewLines = null;

    public Queue<GameObject> lineQueue = new Queue<GameObject>();

    public float lineSpeed = 2f;
    public float appliedLineSpeed = 2f;
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
            NewLines.transform.position = new Vector3(0,-10,0);
            lineQueue.Enqueue(NewLines);
            distance = 0;
        }
        distance = NewLines.transform.position.y - StartPosition.y;

    }

    GameObject MakeLinesPlay(){
        LinesObj = Random.Range(1,3);
        GameObject mNewLines = null;

        switch (LinesObj)
        {
            case 1:
                mNewLines = Managers.Resource.Instantiate(lev1, transform.position);
                break;
            case 2:
                mNewLines = Managers.Resource.Instantiate(lev2, transform.position);
                break;
        }
        LinesMove linesMove = mNewLines.GetComponent<LinesMove>();
        
        linesMove.speed = appliedLineSpeed;
        return mNewLines;
    }

    public void BoostLineSpeed(float time, float force)
    {
        StopCoroutine("BoostLineSpeedCoroutine");
        StartCoroutine(BoostLineSpeedCoroutine(time, force));
    }

    IEnumerator BoostLineSpeedCoroutine(float time, float force)
    {
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
