using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Items
{
    public float invinciblityTime = 10f;
    protected override void ItemEvent()
    {
        CameraController.CameraShakeEvent(invinciblityTime, 1f);
        monkey.StartInvinvible(invinciblityTime);
    }
}
