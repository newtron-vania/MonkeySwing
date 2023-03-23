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
        int itemNum = (int)RandomItem();
        if (itemNum > items.Length)
            return;
        GameObject item = items[itemNum];
        Debug.Log($"itemname : {item.name}, position : {this.transform.position}");
        Managers.Resource.Instantiate(item, this.transform.position, transform.parent);
    }

    Define.Items RandomItem()
    {
        float rand = Random.Range(0, 1f) * 100;
        if (rand < 60)
            return Define.Items.None;
        else if (rand < 80)
            return Define.Items.CaloryBanana;
        else if (rand < 90)
            return Define.Items.Boost;
        else if (rand <= 100)
            return Define.Items.Magnet;
        else
            return Define.Items.None;
    }
}
