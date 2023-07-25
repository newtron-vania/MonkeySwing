using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyStat : MonoBehaviour
{
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    private int weight;
    public int Weight { get { return weight; } }
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    public void SetMonkeyStat()
    {
        SkinDataSO skinData = Managers.Data.GetSkin(GameManagerEx.Instance.player.MonkeySkinId);
        hp = skinData.Hp;
        weight = skinData.Weight;
        speed = skinData.Speed;
    }
}
