using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    int money = 0;

    public int Money { get { return money; } set { money = value; PlayerPrefs.SetInt("Money", money); } }

    float monkeySkinId = 0;
    public float MonkeySkinId { get { return monkeySkinId; } set { monkeySkinId = value; PlayerPrefs.SetFloat("MonkeySkinId", monkeySkinId); } }


    public void SetData()
    {
        Money = PlayerPrefs.GetInt("Money");
        MonkeySkinId = PlayerPrefs.GetFloat("MonkeySkinId");
    }

}
