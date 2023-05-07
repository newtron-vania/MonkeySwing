using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using AssetKits.ParticleImage;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI rewardText;
    [SerializeField]
    Image BestScoreSticker;

    [SerializeField]
    ParticleImage particle;

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
            Managers.Sound.Play("BestScore");
            StartParticle();
            BestScoreSticker.gameObject.SetActive(true);
            GameManagerEx.Instance.player.BestScore = GameManagerEx.Instance.distance.Dist;
            ceremonyAnime.SetInteger("IsBest", 2);
            Debug.Log($"bestScore : {GameManagerEx.Instance.player.BestScore}");
            return;
        }
        Managers.Sound.Play("GameOver");
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
        AdmobManager.Instance.ShowRewardAd(1, (sender, rewardEvent) => { StartContinue(); });
    }

    public void ShowAdsWithBanana()
    {
        AdmobManager.Instance.ShowRewardAd(0, (sender, rewardEvent) => { GiveMulCoin(); });
    }

    private void GiveMulCoin()
    {
        Debug.Log("ShowGiveBananaAd Complete!");
        BananaCount.bananacount *= 2;
        AddBanana();
    }

    private void StartContinue()
    {
        Debug.Log("ShowContinueAd Complete!");
        continueButton.interactable = false;
    }

    private void StartParticle()
    {
        StartCoroutine(StartParticleCoroutine());
    }

    IEnumerator StartParticleCoroutine()
    {
        yield return null;
        particle.gameObject.SetActive(true);
        particle.Play();
    }
}
