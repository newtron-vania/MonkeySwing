using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
