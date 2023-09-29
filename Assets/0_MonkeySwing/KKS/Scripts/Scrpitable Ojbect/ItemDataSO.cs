using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemDataSO : ScriptableObject
{
    [SerializeField]
    public Define.Items itemName;
    [SerializeField]
    public float size;
    [SerializeField]
    public int time;
    [SerializeField]
    public int value;

    public Action ItemEvent;

    public void SetItemEvent(Action eventHandler)
    {
        ItemEvent -= eventHandler;
        ItemEvent += eventHandler;
    }


    public void Clear()
    {
        ItemEvent = null;
    }
}
