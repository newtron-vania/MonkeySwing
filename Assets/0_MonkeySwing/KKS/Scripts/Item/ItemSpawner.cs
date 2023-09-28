using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] items;

 
    [SerializeField]
    Define.Items lockItem = Define.Items.None;

    private void OnEnable()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        if(lockItem < Define.Items.Count)
        {
            Managers.Resource.Instantiate(items[(int)lockItem], this.transform.position, transform.parent);
            return;
        }

        Define.Items item = GameManagerEx.Instance.itemManager.SpawnItem();
        Collider2D[] goes = Physics2D.OverlapCircleAll(this.transform.position, 0.5f);
        foreach(Collider2D collision in goes)
        {
            if (collision.tag == "Banana")
            {
                collision.gameObject.SetActive(false);
            }
        }

        GameObject itemObj = items[(int)item];
        Managers.Resource.Instantiate(itemObj, this.transform.position, transform.parent);
    }
}
