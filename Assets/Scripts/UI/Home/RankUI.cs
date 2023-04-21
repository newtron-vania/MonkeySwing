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
            (success, mydata, data) => { ShowRankScoreData(success, mydata, data); }) ;
    }


    void ShowRankScoreData(bool success, UserRankData mydata, UserRankData[] data)
    {

        if (success)
        {
            myRankNum = mydata.rank;
            myRankItem.SetData(mydata.userName, mydata.userScore.ToString());
            int i = 0;
            foreach(var score in data)
            {
                rankItems[i].SetActive(true);
                RankItemController rankItem = rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score.rank, score.rank == myRankNum, score.userName, score.userScore.ToString());
            }
        }
        else
        {
            myRankItem.SetData("loadError", "loadError");
            errorText.gameObject.SetActive(true);
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
