using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
