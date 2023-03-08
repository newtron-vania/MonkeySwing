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

    void Start() {
        StartPosition = new Vector3(0,-10,0);
        EndPosition = new Vector3(0,10,0);
    }
    // Update is called once per frame
    void Update()
    {
        if(distance >= 10.0f){
            NewLines = MakeLinesPlay();
            //Debug.Log("create success");
            NewLines.transform.position = new Vector3(0,-10,0);
            distance = 0;
        }
        distance = NewLines.transform.position.y - StartPosition.y;
        //Debug.Log(distance);
    }

    GameObject MakeLinesPlay(){
        LinesObj = Random.Range(1,3);
        GameObject mNewLines = null;

        switch (LinesObj)
        {
            case 1:
                mNewLines = Instantiate(lev1, transform.position, Quaternion.identity);
                break;
            case 2:
                mNewLines = Instantiate(lev2, transform.position, Quaternion.identity);
                break;
        
        }
        return mNewLines;
    }
}
