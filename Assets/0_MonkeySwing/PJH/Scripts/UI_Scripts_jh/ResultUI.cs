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
    CountUI countUI;

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

        int bestscore = GameManagerEx.Instance.scoreData.GetScore(GameManagerEx.Instance.mapID);
        Debug.Log($" score : {GameManagerEx.Instance.distance.Dist}, BestScore : {bestscore}");


        if (GameManagerEx.Instance.distance.Dist > bestscore)
        {
            Managers.Sound.Play("BestScore");
            StartParticle();
            BestScoreSticker.gameObject.SetActive(true);
            GameManagerEx.Instance.scoreData.SetScore(GameManagerEx.Instance.mapID, GameManagerEx.Instance.distance.Dist);
            ceremonyAnime.SetInteger("IsBest", 2);
            Debug.Log($"bestScore : {GameManagerEx.Instance.scoreData.GetScore(GameManagerEx.Instance.mapID)}");
            return;
        }
        Managers.Sound.Play("GameOver");
        ceremonyAnime.SetInteger("IsBest", 1);


    }

    public void OnClickRetryButton()
    {
        Managers.Sound.StopPlayingSound(Define.Sound.Effect);
        gameObject.SetActive(false);
        countUI.gameObject.SetActive(true);
        countUI.SetCount(3f);

        GameManagerEx.Instance.monkey.SetMonkeyStat();
    }

    public void AddBanana()
    {
        GameManagerEx.Instance.player.Money += BananaCount.bananacount;
        BananaCount.bananacount = 0;
    }

    public void ShowAdsWithContinue()
    {
        //AdmobManager.Instance.ShowRewardAd(1, (sender, rewardEvent) => { StartContinue(); });
        AdsInitializer.Instance.ShowRewardAd(() => { StartContinue(); OnClickRetryButton(); });
    }

    public void ShowAdsWithBanana()
    {
        //AdmobManager.Instance.ShowRewardAd(0, (sender, rewardEvent) => { GiveMulCoin(); });
        AdsInitializer.Instance.ShowRewardAd(() => { GiveMulCoin(); LoadingScene.LoadScene(Define.SceneType.Home); });
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
