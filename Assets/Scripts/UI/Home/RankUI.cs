using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{

    [SerializeField]
    RankItemController myRankItem;

    [SerializeField]
    private int myRankNum;

    [SerializeField]
    GameObject[] rankItems;

    [SerializeField]
    TextMeshProUGUI errorText;

    

    // Start is called before the first frame update
    void OnEnable()
    {
        GooglePlayManager.Instance.LoadBestScoreRankingArray(10, GooglePlayGames.BasicApi.LeaderboardTimeSpan.AllTime,
            (success, data) => { ShowMyRankScoreData(success, data); },
            (success, data) => { ShowRankScoreData(success, data); }) ;
    }


    void ShowRankScoreData(bool success, GooglePlayGames.BasicApi.LeaderboardScoreData data)
    {
        if (success)
        {
            var scores = data.Scores;
            int i = 0;
            foreach(var score in scores)
            {
                rankItems[i].SetActive(true);
                RankItemController rankItem = rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score.rank, score.rank == myRankNum, score.userID, score.value.ToString());
            }
        }
        else
        {
            errorText.gameObject.SetActive(true);
        }
    }


    void ShowMyRankScoreData(bool success, GooglePlayGames.BasicApi.LeaderboardScoreData data)
    {
        if (success)
        {
            var scores = data.Scores;
            myRankNum = scores[0].rank;
            myRankItem.SetData(scores[0].userID, scores[0].value.ToString());
        }
        else
        {
            myRankItem.SetData("loadError", "loadError");
        }
    }

    public void ClearRank()
    {
        foreach (GameObject item in rankItems)
            item.SetActive(false);
        errorText.gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        ClearRank();
        gameObject.SetActive(false);
    }
}
