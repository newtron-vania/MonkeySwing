using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Items
{
    public float invinciblityTime = 10f;
    public float boostForce = 2f;
    protected override void ItemEvent()
    {
        CameraController.CameraShakeEvent(invinciblityTime, 1f);
        monkey.StartInvinvible(invinciblityTime);
        MakeLines lineGenerator = GameManagerEx.Instance.makeLines;
        lineGenerator.BoostLineSpeed(invinciblityTime, boostForce);
    }
}
