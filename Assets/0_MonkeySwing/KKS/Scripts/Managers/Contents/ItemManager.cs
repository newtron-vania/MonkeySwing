using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Rito.WeightedRandomPicker<Define.Items> pkItem;
    private Rito.WeightedRandomPicker<bool> pkSpawn;
    private int[] weights = { 451, 301, 251 };

    public void Init()
    {
        pkItem = new Rito.WeightedRandomPicker<Define.Items>();
        foreach(Define.Items item in Enum.GetValues(typeof(Define.Items)))
        {
            pkItem.Add(item, weights[(int)item]);
        }
        pkSpawn.Add(false, 900);
        pkSpawn.Add(true, 100);
    }
    private void Modify(Define.Items item, int value = 50)
    {
        pkItem.ModifyWeight(item, value);
    }

    private double GetWeight(Define.Items item)
    {
        return pkItem.GetWeight(item);
    }

    private bool IsBeReset()
    {
        return pkItem.SumOfWeights > 20;
    }

    private bool isSpawn()
    {
        return pkSpawn.GetRandomPick();
    }
    private Define.Items Spawn()
    {
        Define.Items item = pkItem.GetRandomPick();
        Modify(item, (int)pkItem.GetWeight(item));
        return item;
    }
    public Define.Items SpawnItem()
    {
        Define.Items item = Define.Items.None;
        if(pkItem == null)
        {
            Init();
        }
        if (isSpawn())
        {
            item = Spawn();

            if (IsBeReset())
            {
                Init();
            }
        }
        return item;
    }
}
