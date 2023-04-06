using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{

    protected Rigidbody2D rigid;

    private void OnEnable()
    {
        rigid = this.GetComponent<Rigidbody2D>();
    }
    protected abstract void ItemEvent();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monkey")
        {
            ItemEvent();
            Managers.Resource.Destroy(this.gameObject);
        }
    }

}
