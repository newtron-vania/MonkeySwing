using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Items
{
    protected override void ItemEvent()
    {
        BananaCount.bananacount += 1;
    }
}
