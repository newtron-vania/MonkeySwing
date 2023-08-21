using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    public int mapid = 1;

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
            (success, data) => { ShowRankScoreData(success,  data); }, mapid) ;
    }


    void ShowRankScoreData(bool success, List<UserRankData> data)
    {
        //Set My Rank Score
        PlayerData player = GameManagerEx.Instance.player;
        string userName = GooglePlayManager.Instance.LocalUser;
        myRankItem.SetMyData(userName, GameManagerEx.Instance.scoreData.GetScore(mapid), GameManagerEx.Instance.player.MonkeySkinId, mapid);
        if (success)
        {
            int i = 0;
            foreach(var score in data)
            {
                rankItems[i].SetActive(true);
                RankItemController rankItem = rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score, score.userName == userName, mapid);
                i++;
            }
        }
        else
        {
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
