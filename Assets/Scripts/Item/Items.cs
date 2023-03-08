using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    [SerializeField]
    protected MonkeyController monkey;

    private void Start()
    {
        monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
    }
    protected abstract void ItemEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monkey")
        {
            ItemEvent();
            Destroy(this.gameObject);
        }
    }
}
