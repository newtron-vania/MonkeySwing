using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    int money = 0;

    public int Money { get { return money; } set { money = value; } }

    float monkeySkinId = 0;
    public float MonkeySkinId { get { return monkeySkinId; } set { monkeySkinId = value; } }


    public void SetData()
    {
        Money = PlayerPrefs.GetInt("Money");
        MonkeySkinId = PlayerPrefs.GetInt("MonkeySkinId");
    }

}
