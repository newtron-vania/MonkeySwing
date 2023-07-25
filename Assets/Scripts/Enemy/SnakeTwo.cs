using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTwo : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trainSpeed;


    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    public GameObject body;
    public Transform[] bodyParts;

    public Vector3 startPoint;
    private void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        //bodyParts = new Transform[length-1];
        //for(int i = 0; i<bodyParts.Length; i++)
        //{
        //    bodyParts[i] = Instantiate(body).transform;
        //    if(i == 0)
        //    {
        //        bodyParts[i].GetComponent<BodyRotation>().target = this.transform.parent;
        //        continue;
        //    }
        //    bodyParts[i].GetComponent<BodyRotation>().target = bodyParts[i - 1];
        //}
        startPoint = targetDir.position;
        ResetPosition();
    }

    private void Update()
    {
        SetLinePos();
    }

    void SetLinePos()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            //bodyParts[i - 1].transform.position = segmentPoses[i];
        }
        lineRend.SetPositions(segmentPoses);
    }


    private void OnDisable()
    {
        ResetPosition();
    }

    void ResetPosition()
    {
        for(int i=0; i<segmentPoses.Length; i++)
        {
            segmentPoses[i] = startPoint;
        }
        lineRend.SetPositions(segmentPoses);
    }
}
