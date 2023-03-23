using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Items
{
    Vector3 startPoint;

    protected override void ItemEvent()
    {
        BananaCount.bananacount += 1;
    }

    private void Awake()
    {
        startPoint = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.localPosition = startPoint;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monkey")
        {
            ItemEvent();
            Managers.Sound.Play("Coin");
            this.gameObject.SetActive(false);
        }
    }


}
