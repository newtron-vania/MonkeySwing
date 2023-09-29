using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryBanana : Items
{
    protected override void ItemEvent()
    {
        GameManagerEx.Instance.monkey.Weight = Mathf.Min(GameManagerEx.Instance.monkey.Weight + Managers.Data.itemDict["CaloryBanana"].value, GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[3]);
        //Todo
        Managers.Sound.Play("CaloryBanana");
    }

}
