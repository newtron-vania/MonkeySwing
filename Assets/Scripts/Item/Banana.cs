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

    private void Start()
    {
        startPoint = transform.position;
    }

    private void OnEnable()
    {
        transform.position = startPoint;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monkey")
        {
            ItemEvent();
            Managers.Sound.Play("Coint");
            this.gameObject.SetActive(false);
            
        }
    }


}
