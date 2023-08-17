using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Items
{
    protected override void ItemEvent()
    {
        GameObject go = null;
        //CameraController.CameraShakeEvent(invinciblityTime, 1f);
        if (GameObject.FindWithTag("BoostEffect"))
        {
            go = GameObject.FindWithTag("BoostEffect");
        }
        else
        {
            go = Managers.Resource.Instantiate("Item/BoostEffect");
        }

        go.GetComponent<BoostEffect>().ResetTime();
    }
}
