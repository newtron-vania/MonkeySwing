using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class IntertitalAds : IUnityAdsLoadListener, IUnityAdsShowListener
{

    private float time;
    private int count;

    private float limitTime = 180;
    private float limitCount = 5;

    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    Action intertitialEvent;


    void Init()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }


    public void OnUpdate()
    {
        time += Time.unscaledDeltaTime;
    }

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        Init();
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd(Action intertitialAction = null)
    {
        count++;
        intertitialEvent = intertitialAction;
        Debug.Log("Showing Ad: " + _adUnitId);
        if (count >= limitCount || (count >= 1 && time >= limitTime))
        {
            Debug.Log("ShowIntertitialAds Complete!");
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            intertitialEvent.Invoke();
        }
        
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        intertitialEvent.Invoke();
        LoadAd();
        time = 0f;
        count = 0;
    }
}
