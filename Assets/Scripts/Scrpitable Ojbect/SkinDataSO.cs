using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Skin Data", menuName = "Scriptable Object/Skin Data", order = int.MaxValue)]
public class SkinDataSO : ScriptableObject
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
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    private int weight;
    public int Weight { get { return weight; } }
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }
}
