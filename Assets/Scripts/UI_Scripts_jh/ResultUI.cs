using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI rewardText;
    [SerializeField]
    Image BestScoreSticker;

    private void OnEnable()
    {
        scoreText.text = $"{GameManagerEx.Instance.distance.Dist.ToString()}m";
        rewardText.text = BananaCount.bananacount.ToString();
        Debug.Log($" score : {GameManagerEx.Instance.distance.Dist}, BestScore : {GameManagerEx.Instance.player.BestScore}");
        if (GameManagerEx.Instance.distance.Dist > GameManagerEx.Instance.player.BestScore)
        {
            BestScoreSticker.gameObject.SetActive(true);
            GameManagerEx.Instance.player.BestScore = GameManagerEx.Instance.distance.Dist;
            Debug.Log($"bestScore : {GameManagerEx.Instance.player.BestScore}");
        }
    }
    public void ShowAds()
    {
        GiveMulCoin();
    }

    private void GiveMulCoin()
    {
        BananaCount.bananacount *= 2;
    }
}
