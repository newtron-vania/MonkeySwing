using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankItemController : MonoBehaviour
{
    [SerializeField]
    Image rankPanel;

    [SerializeField]
    private TextMeshProUGUI idText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    string basicPath = "UI/UI_rank/rankPanel/";
    string brightPath = "myrank_";
    string dartPath = "rank_";

    public void SetData(int rank, bool myRank, string id, string score)
    {
        string path = basicPath;

        if (myRank)
            path += brightPath + rank.ToString();
        else
            path += dartPath + rank.ToString();
        rankPanel.sprite = Managers.Resource.LoadSprite(path);
        idText.text = id;
        scoreText.text = score + "M";
    }

    public void SetData(string id, string score)
    {
        idText.text = id;
        scoreText.text = score + "M";
    }
}
