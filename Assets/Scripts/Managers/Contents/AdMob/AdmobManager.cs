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

    [SerializeField]
    private float limitTime = 300f;


    static void Init()
    {
        if (instance == null)
        {

            //매니저 초기화
            GameObject go = GameObject.Find("@AdmobManager");
            if (go == null)
            {
                go = new GameObject { name = "@AdmobManager" };
                go.AddComponent<AdmobManager>();
            }
            //삭제되지 않게끔 설정 -> Scene 이동을 하더라도 파괴되지 않음
            DontDestroyOnLoad(go);
            instance = go.GetComponent<AdmobManager>();
        }
    }

    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        LoadFrontAd();
        LoadRewardAd(null);
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
        if (time > limitTime)
        {
            if (this.frontAd.IsLoaded())
            {
                frontAd.Show();
            }
        }
    }
    #endregion



    #region 리워드 광고
    const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "ca-app-pub-5201529208326516/9990033838";
    RewardedAd rewardAd;


    void LoadRewardAd(EventHandler<Reward> rewardEvent)
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward -= rewardEvent;
        rewardAd.OnUserEarnedReward += rewardEvent;

    }

    public void ShowRewardAd(EventHandler<Reward> rewardEvent)
    {
        if (rewardAd.IsLoaded())
        {
            LoadRewardAd(rewardEvent);
            rewardAd.Show();
        }
            
    }
    #endregion
}
