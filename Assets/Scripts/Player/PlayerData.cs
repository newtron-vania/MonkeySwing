using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [SerializeField]
    int money = 0;

    public int Money { get { return money; } set { money = value; SetData(); } }
    [SerializeField]
    float currentSkinId = 0;
    public float MonkeySkinId { get { return currentSkinId; } set { currentSkinId = value; SetData(); } }
    [SerializeField]
    List<int> collectedSkinId = new List<int>();
    [SerializeField]
    int bestScore = 0;
    public int BestScore { get { return bestScore; } set { bestScore = value; SetData(); UploadToBestScoreRanking(); } }


    private void SetData()
    {
        GooglePlayManager.Instance.SaveToCloud(JsonUtility.ToJson(GameManagerEx.Instance.player));
    }

    public void LoadData()
    {
        GooglePlayManager.Instance.LoadFromCloud((data) => { GameManagerEx.Instance.player = JsonUtility.FromJson<PlayerData>(data); });
    }

    private void UploadToBestScoreRanking()
    {
        GooglePlayManager.Instance.ReportLeaderboard(GPGSIds.leaderboard_bestscore, BestScore, null);
        Debug.Log("Upload LeaderBoard Complete!");
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
