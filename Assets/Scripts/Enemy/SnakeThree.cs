using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeThree : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;

    public GameObject body;
    public Transform[] bodyParts;

    public Vector3 startPoint;

    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        startPoint = targetDir.position;
        ResetPosition();

        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetLinePos();
    }


    private void OnEnable()
    {
        anime.Play("snakeMove");
    }
    void SetLinePos()
    {
        segmentPoses[0] = targetDir.position;

        float dir = (segmentPoses[0] - segmentPoses[1]).magnitude;
        if (dir < targetDist)
            return;
        for (int i = segmentPoses.Length-1; i >= 1; i--)
        {
            segmentPoses[i] = segmentPoses[i - 1];
        }
        lineRend.SetPositions(segmentPoses);
    }
    void ResetPosition()
    {
        for (int i = 0; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = targetDir.position;
        }
        lineRend.SetPositions(segmentPoses);
    }
}
