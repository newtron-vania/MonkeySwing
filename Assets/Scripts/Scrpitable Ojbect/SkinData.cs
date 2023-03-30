using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Skin Data", menuName = "Scriptable Object/Skin Data", order = int.MaxValue)]
public class SkinData : ScriptableObject
{
    [SerializeField]
    private int skinId;
    public int SkinId { get { return skinId; }}
    [SerializeField]
    private Define.Rarelity rarelity;
    public Define.Rarelity Rarelity { get { return rarelity; }}
    [SerializeField]
    private Sprite skinTale;
    public Sprite SkinTale { get { return skinTale; }}
    [SerializeField]
    private Sprite skinWood;
    public Sprite SkinWood { get { return skinWood; }}
    [SerializeField]
    private Sprite skinHead;
    public Sprite SkinHead { get { return skinHead; }}

    [SerializeField]

    private int cost;
    public int Cost { get { return cost; } }
}
