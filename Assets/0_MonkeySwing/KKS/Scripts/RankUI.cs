using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    public int _mapid = 1;

    [SerializeField]
    private RankItemController _myRankItem;

    [SerializeField]
    private int _myRankNum;

    [SerializeField]
    private GameObject[] _rankItems;

    [SerializeField]
    private TextMeshProUGUI _errorText;

    

    // Start is called before the first frame update
    void OnEnable()
    {
        GooglePlayManager.Instance.LoadBestScoreRankingArray2(10,
            (success, data) => { ShowRankScoreData(success,  data); }, _mapid) ;
    }


    void ShowRankScoreData(bool success, List<UserRankData> data)
    {
        //Set My Rank Score
        PlayerData player = GameManagerEx.Instance.player;
        string userName = GooglePlayManager.Instance.LocalUser;
        _myRankItem.SetMyData(userName, GameManagerEx.Instance.scoreData.GetScore(_mapid), player.MonkeySkinId, _mapid);
        if (success)
        {
            int i = 0;
            foreach(var score in data)
            {
                _rankItems[i].SetActive(true);
                RankItemController rankItem = _rankItems[i].GetComponent<RankItemController>();
                rankItem.SetData(score, score.userName == userName, _mapid);
                i++;
            }
        }
        else
        {
            _errorText.gameObject.SetActive(true);
        }
    }

    public void ClearRank()
    {
        foreach (GameObject item in _rankItems)
            item.SetActive(false);
        _errorText.gameObject.SetActive(false);
    }

    public void OnClickClose()
    {
        ClearRank();
        gameObject.SetActive(false);
    }
}
