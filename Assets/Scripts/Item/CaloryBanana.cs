using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaloryBanana : Items
{
    protected override void ItemEvent()
    {
        monkey.weight = Mathf.Min(monkey.weight + 20, 100);
    }

}
