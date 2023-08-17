using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    CaloryBanana,
    Magnet,
    Booster
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data", order = int.MaxValue)]
public class ItemDataSO : ScriptableObject
{
    [SerializeField]
    public Item itemName;
    [SerializeField]
    public float size;
    [SerializeField]
    public int time;
    [SerializeField]
    public int value;
}
