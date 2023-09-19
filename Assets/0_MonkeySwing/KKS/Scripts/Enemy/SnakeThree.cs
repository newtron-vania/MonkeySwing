using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeThree : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform SnakeModel;
    public Transform targetDir;
    public float targetDist;

    public Vector3 startPoint;

    [SerializeField]
    private Animator anime;
    [SerializeField]
    private GenerateCollider generateCollider;

    private Vector3 dirVec;

    public bool testing = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        startPoint = targetDir.position - SnakeModel.position;

        dirVec = transform.parent.localScale;

        if (testing)
            return;
        GameManagerEx.Instance.makeLines.lineSpeedAction -= SetAnimeSpeed;
        GameManagerEx.Instance.makeLines.lineSpeedAction += SetAnimeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SetLinePos();
        generateCollider.SetEdgeCollider(lineRend);
    }
    private void OnEnable()
    {
        anime.Play("None");
    }

    void SetAnimeSpeed(float value)
    {
        anime.speed = value * 0.5f;
    }

    void SetLinePos()
    {
        //
        Vector3 movePos = targetDir.position - SnakeModel.position;
        segmentPoses[0] =  new Vector3(movePos.x * dirVec.x, movePos.y * dirVec.y, 0f);
        float dir = (segmentPoses[0] - segmentPoses[1]).magnitude;
        if (dir < targetDist)
            return;
        for (int i = segmentPoses.Length-1; i >= 1; i--)
        {
            segmentPoses[i] = segmentPoses[i - 1];
        }
        lineRend.SetPositions(segmentPoses);
    }
    public void ResetPosition()
    {
        for (int i = 0; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = targetDir.position - SnakeModel.position;
        }
        lineRend.SetPositions(segmentPoses);
    }

    void lastSetLinePos()
    {
        Vector3 movePos = targetDir.position - SnakeModel.position;
        segmentPoses[0] = new Vector3(movePos.x * dirVec.x, movePos.y * dirVec.y, 0f);

        for (int i = segmentPoses.Length - 1; i >= 1; i--)
        {
            segmentPoses[i] = segmentPoses[i - 1];
        }
        lineRend.SetPositions(segmentPoses);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TriggerZone"))
        {
            anime.Play("snakeMove");
        }
    }
}
