using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryBanana : Items
{
    protected override void ItemEvent()
    {
        monkey.Weight = Mathf.Min(monkey.Weight + 20, 100);
        //Todo
        Managers.Sound.Play("Coin");
    }

}
