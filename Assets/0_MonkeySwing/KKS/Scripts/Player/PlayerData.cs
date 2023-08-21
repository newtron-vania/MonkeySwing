using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [SerializeField]
    private string username = string.Empty;

    public string UserName { get { if (username == string.Empty) username = GooglePlayManager.Instance.LocalUser; return username; } }
    [SerializeField]
    int money = 0;

    public int Money { get { return money; } set { money = value; SetData(); if (GameManagerEx.Instance.currentCoin > money) GameManagerEx.Instance.currentCoin = money; } }
    [SerializeField]
    int currentSkinId = 0;
    public int MonkeySkinId { get { return currentSkinId; } set { currentSkinId = value; SetData(); } }
    [SerializeField]
    List<int> collectedSkinId = new List<int>() { 0 };
    [SerializeField]
    int bestScore = 0;
    public int BestScore { get { return -1 * bestScore; } set { bestScore = -1 * value; SetData(); } }


    private void SetData()
    {
        if (username == string.Empty) username = GooglePlayManager.Instance.LocalUser;
        GooglePlayManager.Instance.SaveUser(JsonUtility.ToJson(GameManagerEx.Instance.player));
        GooglePlayManager.Instance.SaveScore(DictionaryJsonUtility.ToJson(GameManagerEx.Instance.scoreData.BestScore, false));
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
}
