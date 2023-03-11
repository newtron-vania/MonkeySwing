using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Items
{
    [SerializeField]
    private bool Magneting = false;
    protected override void ItemEvent()
    {
        BananaCount.bananacount += 1;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.tag == "MagnetField" && !Magneting)
        {
            Magneting = true;
            StartCoroutine(MoveToPlayer());
            Debug.Log("Banana is MagnetField!");
        }
    }

    IEnumerator MoveToPlayer()
    {
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, monkey.transform.position, 0.05f);
            yield return new WaitForFixedUpdate();
        }
    }
}
