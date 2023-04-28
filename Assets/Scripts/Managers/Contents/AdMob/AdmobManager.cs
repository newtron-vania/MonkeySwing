using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class AdmobManager : MonoBehaviour
{
    private static AdmobManager instance;

    public static AdmobManager Instance { get { Init(); return instance; } }

    public bool isTestMode = true;


    private float time = 0f;
    private int count = 0;

    [SerializeField]
    private float limitTime = 120f;
    [SerializeField]
    private int maxCount = 3;


    public void call()
    {

    }

    static void Init()
    {
        if (instance == null)
        {
            //매니저 초기화
            GameObject go = GameObject.Find("AdmobManager");
            if (go == null)
            {
                go = new GameObject { name = "AdmobManager" };
                go.AddComponent<AdmobManager>();
            }
            //삭제되지 않게끔 설정 -> Scene 이동을 하더라도 파괴되지 않음
            DontDestroyOnLoad(go);
            instance = go.GetComponent<AdmobManager>();
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        LoadFrontAd();
        LoadBananaRewardAd();
        LoadContinueRewardAd();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }


    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-5201529208326516/4708914858";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
    }

    public void ShowFrontAd()
    {
        if (time > limitTime || count >= maxCount)
        {
            if (this.frontAd.IsLoaded())
            {
                frontAd.Show();
                time = 0f;
                count = 0;
            }
        }
        else
        {
            Debug.Log($"Ad time = {time}, count = {count}");
        }
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string bananaRewardID = "ca-app-pub-5201529208326516/9990033838";
    const string continueRewardID = "ca-app-pub-5201529208326516/8829598046";
    RewardedAd bananaRewardAd;
    RewardedAd continueRewardAd;


    void LoadBananaRewardAd()
    {
        bananaRewardAd = new RewardedAd(isTestMode ? rewardTestID : bananaRewardID);
        bananaRewardAd.LoadAd(GetAdRequest());
    }

    void LoadContinueRewardAd()
    {
        continueRewardAd = new RewardedAd(isTestMode ? rewardTestID : continueRewardID);
        continueRewardAd.LoadAd(GetAdRequest());
    }

    public void ShowRewardAd(int num, EventHandler<Reward> rewardEvent)
    {
        switch (num)
        {
            case 0:
                if (bananaRewardAd.IsLoaded())
                {
                    bananaRewardAd.OnUserEarnedReward -= rewardEvent;
                    bananaRewardAd.OnUserEarnedReward += rewardEvent;
                    bananaRewardAd.Show();
                    return;
                }
                break;
            case 1:
                if (continueRewardAd.IsLoaded())
                {
                    continueRewardAd.OnUserEarnedReward -= rewardEvent;
                    continueRewardAd.OnUserEarnedReward += rewardEvent;
                    continueRewardAd.Show();
                    return;
                }
                break;
        }
        
        Debug.Log("Cannot Loaded RewardAd");
    }
    #endregion
}
