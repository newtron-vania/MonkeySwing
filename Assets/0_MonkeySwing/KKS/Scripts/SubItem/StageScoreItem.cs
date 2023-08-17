using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageScoreItem : MonoBehaviour
{
    [SerializeField]
    Image medalImg;
    [SerializeField]
    List<Sprite> medalImgList;
    [SerializeField]
    TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText.text = GameManagerEx.Instance.player.BestScore.ToString();
    }

    private void ShowMedalImg()
    {
        int score = GameManagerEx.Instance.player.BestScore;
        if (score < 100)
            medalImg.sprite = medalImgList[0];
        else if (score < 300)
            medalImg.sprite = medalImgList[1];
        else if (score < 500)
            medalImg.sprite = medalImgList[2];
        else if (score < 1000)
            medalImg.sprite = medalImgList[3];
        else
            medalImg.sprite = medalImgList[4];
    }
}
