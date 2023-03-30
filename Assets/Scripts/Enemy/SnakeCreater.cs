using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCreater : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    private void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
    }

    private void Update()
    {
        segmentPoses[0] = targetDir.position;

        for(int i=1; i<segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i-1] + targetDir.right * targetDist,ref segmentV[i], smoothSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}
