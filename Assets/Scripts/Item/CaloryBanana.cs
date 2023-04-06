using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryBanana : Items
{
    protected override void ItemEvent()
    {
        GameManagerEx.Instance.monkey.Weight = Mathf.Min(GameManagerEx.Instance.monkey.Weight + 20, 100);
        //Todo
        Managers.Sound.Play("CaloryBanana");
    }

}
