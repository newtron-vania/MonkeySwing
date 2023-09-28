using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerData
{
    [SerializeField]
    private string username = string.Empty;

    public string UserName { get { if (username == string.Empty) username = GooglePlayManager.Instance.LocalUser; return username; } }
    [SerializeField]
    int money = 0;

    public int Money { get { return money; } set { money = value; SetData(); if (GameManagerEx.Instance.currentCoin > money) GameManagerEx.Instance.currentCoin = money; } }
    [SerializeField]
    private int currentSkinId = 0;
    public int MonkeySkinId { get { return currentSkinId; } set { currentSkinId = value; SetData(); } }
    [SerializeField]
    private List<int> collectedSkinId = new List<int>() { 0 };
    [SerializeField]
    private int bestScore = 0;
    public int BestScore { get { return -1 * bestScore; } set { bestScore = -1 * value; SetData(); } }

    [SerializeField]
    private int[] itemArr = new int[4];

    public bool HasItem(Define.Items item)
    {
        Debug.Log($"{item.ToString()} count : {itemArr[(int)item]} | Has : {itemArr[(int)item] > 0}");
        return itemArr[(int)item] > 0;
    }
    public int GetItem(Define.Items item)
    {
        return itemArr[(int)item];
    }

    public void SetItem(Define.Items item, int value)
    {
        if (value < 0)
            value = 0;
        itemArr[(int)item] = value;
    }

    public bool UseItem(Define.Items item)
    {
        int itemId = (int)item;
        if(itemArr[itemId] > 0)
        {
            itemArr[itemId] -= 1;
            return true;
        }
        return false;
    }

    private void SetData()
    {
        if (username == string.Empty) username = GooglePlayManager.Instance.LocalUser;
        GooglePlayManager.Instance.SaveUser(JsonUtility.ToJson(GameManagerEx.Instance.player));
    }

    public void LoadData()
    {
        GooglePlayManager.Instance.LoadFromCloud(
            (data) => 
            { 
            GameManagerEx.Instance.player = JsonUtility.FromJson<PlayerData>(data); GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.money;
            },
            (data) =>
            {
                GameManagerEx.Instance.scoreData.BestScore = DictionaryJsonUtility.FromJson<string, int>(data);
            }
        );
    }

    public void AddSkinId(int id)
    {
        if (!collectedSkinId.Contains(id))
        {
            collectedSkinId.Add(id);
            SetData();
        }
    }

    public List<int> GetSkinIds()
    {
        return collectedSkinId;
    }

    public void ShowPlayerData()
    {
        Debug.Log($"userName : {UserName}\n currentskinid : {MonkeySkinId} \n money : {Money}");
        foreach(Define.Items item in Enum.GetValues(typeof(Define.Items)))
        {
            Debug.Log($"{item.ToString()} count : {itemArr[(int)item]} | Has : {itemArr[(int)item] > 0}");
        }
    }
}
