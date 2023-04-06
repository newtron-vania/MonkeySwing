using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI rewardText;

    private void OnEnable()
    {
        scoreText.text = $"{GameManagerEx.Instance.distance.Dist.ToString()}m";
        rewardText.text = BananaCount.bananacount.ToString();
        Debug.Log($" score : {GameManagerEx.Instance.distance.Dist}, BestScore : {GameManagerEx.Instance.player.BestScore}");
        if (GameManagerEx.Instance.distance.Dist > GameManagerEx.Instance.player.BestScore)
        {
            GameManagerEx.Instance.player.BestScore = GameManagerEx.Instance.distance.Dist;
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
