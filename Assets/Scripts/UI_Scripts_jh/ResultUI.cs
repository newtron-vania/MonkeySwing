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

    [SerializeField]
    Button continueButton;

    [SerializeField]
    Animator ceremonyAnime;
    private void OnEnable()
    {
        scoreText.text = $"{GameManagerEx.Instance.distance.Dist.ToString()}m";
        rewardText.text = BananaCount.bananacount.ToString();
        Debug.Log($" score : {GameManagerEx.Instance.distance.Dist}, BestScore : {GameManagerEx.Instance.player.BestScore}");


        if (GameManagerEx.Instance.distance.Dist > GameManagerEx.Instance.player.BestScore)
        {
            BestScoreSticker.gameObject.SetActive(true);
            GameManagerEx.Instance.player.BestScore = GameManagerEx.Instance.distance.Dist;
            ceremonyAnime.SetInteger("IsBest", 2);
            Debug.Log($"bestScore : {GameManagerEx.Instance.player.BestScore}");
            return;
        }
        ceremonyAnime.SetInteger("IsBest", 1);


        AdmobManager.Instance.ShowFrontAd();
    }

    public void AddBanana()
    {
        GameManagerEx.Instance.player.Money += BananaCount.bananacount;
        BananaCount.bananacount = 0;
    }

    public void ShowAdsWithContinue()
    {
        AdmobManager.Instance.ShowRewardAd((sender, rewardEvent) => { StartContinue(); });
    }

    public void ShowAdsWithBanana()
    {
        AdmobManager.Instance.ShowRewardAd((sender, rewardEvent) => { GiveMulCoin(); });
    }

    private void GiveMulCoin()
    {
        BananaCount.bananacount *= 2;
        AddBanana();
    }

    private void StartContinue()
    {
        continueButton.gameObject.SetActive(false);
        
    }
}
