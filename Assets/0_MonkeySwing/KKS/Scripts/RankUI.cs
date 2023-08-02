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
        GooglePlayManager.Instance.LoadBestScoreRankingArray(10,
            (success, data) => { ShowRankScoreData(success,  data); }) ;
    }


    void ShowRankScoreData(bool success, List<UserRankData> data)
    {
        //Set My Rank Score
        PlayerData player = GameManagerEx.Instance.player;
        string userName = GooglePlayManager.Instance.LocalUser;
        myRankItem.SetData(userName, player.BestScore.ToString());
        if (success)
        {
            int i = 0;
            foreach(var score in data)
            {
                rankItems[i].SetActive(true);
                RankItemController rankItem = rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score.rank, score.userName == userName, score.userName, score.bestScore.ToString());
                i++;
            }
        }
        else
        {
            errorText.gameObject.SetActive(true);
        }
    }

    void ShowMyRankScoreData(bool success, UserRankData[] data)
    {
        if (success)
        {
            UserRankData mydata = data[0];
            myRankNum = mydata.rank;
            myRankItem.SetData(mydata.userName, mydata.bestScore.ToString());
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
