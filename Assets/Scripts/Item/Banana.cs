using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Items
{
    MagnetingItem magneting;

    protected override void ItemEvent()
    {
        BananaCount.bananacount += 1;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monkey")
        {
            ItemEvent();
            this.gameObject.SetActive(false);
        }
    }


}
