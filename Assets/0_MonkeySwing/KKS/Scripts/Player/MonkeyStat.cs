using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStat : MonoBehaviour
{
    [SerializeField]
    private int hp = 3;
    public int Hp { get { return hp; } set { hp = value; } }
    [SerializeField]
    private int weight = 50;
    public int Weight { get { return weight; } set { weight = value; } }
    [SerializeField]
    private float speed = 2f;
    public float Speed { get { return speed; } set { speed = value; } }


    [SerializeField]
    private int[] weightCut = new int[] { 10, 30, 80, 100 };
    public int[] WeightCut { get { return weightCut; } }

}
