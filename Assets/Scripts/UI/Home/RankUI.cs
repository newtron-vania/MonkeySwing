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
            (success, data) => { ShowMyRankScoreData(success,  data); },
            (success, data) => { ShowRankScoreData(success,  data); }) ;
    }


    void ShowRankScoreData(bool success, UserRankData[] data)
    {

        if (success)
        {
            int i = 0;
            foreach(var score in data)
            {
                rankItems[i].SetActive(true);
                RankItemController rankItem = rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score.rank, score.rank == myRankNum, score.userName, score.userScore.ToString());
                i++;
            }
        }
        else
        {
            myRankItem.SetData("loadError", "loadError");
            errorText.gameObject.SetActive(true);
        }
    }

    void ShowMyRankScoreData(bool success, UserRankData[] data)
    {
        if (success)
        {
            UserRankData mydata = data[0];
            myRankNum = mydata.rank;
            myRankItem.SetData(mydata.userName, mydata.userScore.ToString());
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
