using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankItemController : MonoBehaviour
{
    [SerializeField]
    Image monkeySkin;
    [SerializeField]
    Image medalImg;
    [SerializeField]
    Image rankImg;
    [SerializeField]
    Image panelImg;
    [SerializeField]
    private TextMeshProUGUI idText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    string panelPath = "UI/UI_rank/rankPanel/";
    string rankPath = "UI/UI_rank/Number/";

    string rankBoxMy = "UI_rank_box_my";
    string rankBox = "UI_rank_box";

    public void SetData(UserRankData score, bool myRank, int mapid)
    {
        rankImg.sprite = Managers.Resource.LoadSprite(rankPath + score.rank.ToString());
        Debug.Log(rankImg.sprite.name);

        SkinDataSO skindata = Managers.Data.GetSkin(score.skinID);
        Debug.Log($"{skindata.SkinId}");

        monkeySkin.sprite = skindata.SkinHead;

        idText.text = score.userName;
        scoreText.text = score.bestScore.ToString() + " M";

        SetMedal((int)score.bestScore, mapid);
    }

    public void SetMedal(int score,int mapid)
    {
        medalImg.sprite = Managers.Resource.LoadSprite(Managers.Data.GetMedalSpritePath(score, mapid));
    }
    public void SetMyData(string id, int score, int skinId, int mapid)
    {
        SkinDataSO skindata = Managers.Data.GetSkin(skinId);
        Debug.Log($"{skindata.SkinId}");
        monkeySkin.sprite = skindata.SkinHead;

        idText.text = id;
        scoreText.text = score.ToString() + " M";


        SetMedal(score, mapid);
    }
}
