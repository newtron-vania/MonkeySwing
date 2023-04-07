using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChainCreater : MonoBehaviour
{
    public GameObject chainObject;
    public GameObject startChainedObject;
    public GameObject lastChainedObject;

    public List<GameObject> chainList = new List<GameObject>();

    public Action<List<SpriteRenderer>> createMonkeyEvent;

    [SerializeField]
    float length = 2.5f;
    void Start()
    {
        ChainLastObject();
        //CalculateChainCount();
    }
    
    private void Update()
    {
        Debug.Log($"monkeyController Enabled! : {GameManagerEx.Instance.monkey.enabled}");
    }
    private void ChainLastObject()
    {
        lastChainedObject = Managers.Resource.Instantiate(lastChainedObject, startChainedObject.transform.position, startChainedObject.transform.parent);
        HingeJoint2D hingeJoint2D = lastChainedObject.GetComponent<HingeJoint2D>();
        FixedJoint2D fixedJoint2D = lastChainedObject.GetComponent<FixedJoint2D>();
        hingeJoint2D.connectedBody = startChainedObject.GetComponent<Rigidbody2D>();
        fixedJoint2D.connectedBody = startChainedObject.GetComponent<Rigidbody2D>();
        hingeJoint2D.anchor = new Vector2(0, length);
        fixedJoint2D.anchor = new Vector2(0, length);

        List<SpriteRenderer> skinPositons = lastChainedObject.GetComponent<MonkeyController>().GetSkin();
        skinPositons.Add(GetComponent<SpriteRenderer>());

        createMonkeyEvent.Invoke(skinPositons);
    }

    private void CalculateChainCount()
    {
        float dist = lastChainedObject.GetComponent<DistanceJoint2D>().distance;
        int count = (int)(dist / 0.2f) - 1;
        for (int i = 0; i < count ; i++)
        {
            GameObject chain = Managers.Resource.Instantiate(chainObject, startChainedObject.transform.position, startChainedObject.transform.parent);
            HingeJoint2D hinge = chain.GetComponent<HingeJoint2D>();
            if (i == 0)
            {
                hinge.anchor = new Vector2(0, 1f);
                hinge.connectedBody = startChainedObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                hinge.anchor = new Vector2(0, 1f);
                hinge.connectedAnchor = new Vector2(0, -1f);
                hinge.connectedBody = chainList[i-1].GetComponent<Rigidbody2D>();
            }
            chainList.Add(chain);
        }
        HingeJoint2D lastHinge = lastChainedObject.GetComponent<HingeJoint2D>();
        lastHinge.connectedBody = chainList[count - 1].GetComponent<Rigidbody2D>();
    }



}
