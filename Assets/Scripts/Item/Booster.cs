using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Items
{
    [SerializeField]
    GameObject boostEffect;
    protected override void ItemEvent()
    {
        //CameraController.CameraShakeEvent(invinciblityTime, 1f);
        if (GameObject.FindWithTag("BoostEffect"))
            GameObject.FindWithTag("BoostEffect").GetComponent<BoostEffect>().ResetTime();
        else
        {
            GameObject go = Managers.Resource.Instantiate(boostEffect, boostEffect.transform.position);
            go.transform.parent = monkey.transform.parent.GetChild(0);
        }
        
    }
}
