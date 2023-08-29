using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{


    static private AdsInitializer instance;
    static public AdsInitializer Instance
    {
        get { Init(); return instance; }
    }

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    RewardAds rewardAds = new RewardAds();
    IntertitalAds intertitalAds = new IntertitalAds();

    void Awake()
    {
        Init();
        InitializeAds();
    }

    private void Update()
    {
        intertitalAds.OnUpdate();
    }

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("AdsManager");
            if (go == null)
            {
                go = new GameObject { name = "AdsManager" };
                go.AddComponent<AdsInitializer>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<AdsInitializer>();
        }
    }

    public void ShowRewardAd(Action rewardAction = null)
    {
        rewardAds.ShowAd(rewardAction);
    }

    public void ShowIntertitalAd(Action intertitialAction = null)
    {
        intertitalAds.ShowAd(intertitialAction);
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
            Debug.Log($"android game Id : {instance._gameId}");
            Debug.Log($"Test Mode : {instance._testMode}");
        }
        
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        rewardAds.LoadAd();
        intertitalAds.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
