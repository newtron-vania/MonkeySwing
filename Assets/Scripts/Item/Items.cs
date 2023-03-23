using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{
    [SerializeField]
    protected MonkeyController monkey;

    protected Rigidbody2D rigid;

    private void Start()
    {
        StartCoroutine("FindMonkey");
        rigid = this.GetComponent<Rigidbody2D>();
    }
    protected abstract void ItemEvent();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monkey")
        {
            ItemEvent();
            //Managers.Resource.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator FindMonkey()
    {
        while(GameObject.FindWithTag("Monkey") == null)
        {
            yield return new WaitForFixedUpdate();
        }
        monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
        //Debug.Log($"{this.transform.name} find {monkey.transform.name}");
    }
}
