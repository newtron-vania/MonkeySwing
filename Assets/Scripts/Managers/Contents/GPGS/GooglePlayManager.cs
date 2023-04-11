using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Text;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using System;
using GooglePlayGames.BasicApi.SavedGame;
using TMPro;

public class GooglePlayManager : MonoBehaviour
{
    static public GameObject GPGSUI;
    static TextMeshProUGUI GPGSUIText;
    static private GooglePlayManager instance;
    static public GooglePlayManager Instance { get { Init();  return instance; }
    }

    static GameObject hideUI;

    public bool isProcessing
    {
        get;
        private set;
    }
    public string loadedData
    {
        get;
        private set;
    }

    private const string m_saveFileName = "playerData";
    public bool isAuthenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    static void Init() 
    { 
        if(instance == null)
        {
            GameObject go = GameObject.Find("@GooglePlayManager");
            if(go == null)
            {
                go = new GameObject { name = "@GooglePlayManager" };
                go.AddComponent<GooglePlayManager>();
                hideUI = Managers.Resource.Instantiate("UI/HideUI");
                hideUI.SetActive(false);
                GPGSUI = Managers.Resource.Instantiate("UI/GPGSUI");
                GPGSUIText = GPGSUI.FindChild<TextMeshProUGUI>();
            }
            DontDestroyOnLoad(go);
            DontDestroyOnLoad(hideUI);
            DontDestroyOnLoad(GPGSUI);
            instance = go.GetComponent<GooglePlayManager>();
        }
    }
    private void InitiatePlayGames()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames() // Saved Games 기능 활성화
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Awake()
    {
        InitiatePlayGames();
    }
    public void Login(Action successAction, Action hideUIAction)
    {
        hideUI.SetActive(true);
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log(Social.localUser.id + "\n" + Social.localUser.userName);
            hideUIAction.Invoke();
            if (!success)
            {

                Debug.Log("Fail Login");
            }
            else
            {
                successAction.Invoke();
                Debug.Log("Login Succeed");
            }
            
        });
    }


    private void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null || cloudData.Length < 10)
        {
            Debug.Log("No Data saved to the cloud");
            loadedData = JsonUtility.ToJson(new PlayerData());
            return;
        }
        //TODO : 
        Debug.Log("Load Completed!");
        string progress = BytesToString(cloudData);
        //json File
        loadedData = progress;
    }


    public void LoadFromCloud(Action<string> afterLoadAction)
    {
        
        if (isAuthenticated && !isProcessing)
        {
            StartCoroutine(LoadFromCloudRoutin(afterLoadAction));
        }
        else
        {
            Login(() => StartCoroutine(LoadFromCloudRoutin(afterLoadAction)), () => hideUI.SetActive(false));
        }
    }

    private IEnumerator LoadFromCloudRoutin(Action<string> loadAction)
    {
        isProcessing = true;
        Debug.Log("Loading game progress from the cloud.");
        hideUI.SetActive(true);

        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            m_saveFileName, //name of file.
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            OnFileOpenToLoad);

        while (isProcessing)
        {
            yield return null;
        }

        hideUI.SetActive(false);
        loadAction.Invoke(loadedData);
    }

    public void SaveToCloud(string dataToSave)
    {
        if (isAuthenticated)
        {
            loadedData = dataToSave;
            Debug.Log("loadedData");
            isProcessing = true;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseMostRecentlySaved, OnFileOpenToSave);
        }
        else
        {
            Login(() => SaveToCloud(dataToSave), () => hideUI.SetActive(false));
        }
    }

    private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        hideUI.SetActive(true);
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = StringToBytes(loadedData);

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
        }
        else
        {
            Debug.Log("Error opening Saved Game" + status);
        }
        hideUI.SetActive(false);
    }


    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
        }
        else
        {
            Debug.Log("Error opening Saved Game" + status);
        }
    }


    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.Log("Error Saving" + status);
        }
        else
        {
            ProcessCloudData(bytes);
        }

        isProcessing = false;
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.Log("Error Saving" + status);
        }

        isProcessing = false;
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    public void ReportLeaderboard(string gpgsId, long score, Action<bool> onReported = null) =>
        Social.ReportScore(score, gpgsId, success => onReported?.Invoke(success));

    public void ShowBestScoreLeaderboardUI() =>
        Social.ShowLeaderboardUI();



    public void LoadBestScoreRankingArray(int rowCount, LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, LeaderboardScoreData> onLoadedPlayerRankAction = null, Action<bool, LeaderboardScoreData> onLoadedTopRankAction = null)
    {
        hideUI.SetActive(true);
        LoadCustomLeaderboardArray(GPGSIds.leaderboard_bestscore, 1, LeaderboardStart.PlayerCentered, leaderboardTimeSpan, onLoadedPlayerRankAction);
        LoadCustomLeaderboardArray(GPGSIds.leaderboard_bestscore, rowCount, LeaderboardStart.TopScores, leaderboardTimeSpan, onLoadedTopRankAction);
        hideUI.SetActive(false);
    }
    private void LoadCustomLeaderboardArray(string gpgsId, int rowCount, LeaderboardStart leaderboardStart,
    LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, LeaderboardScoreData> onloaded = null)
    {
        PlayGamesPlatform.Instance.LoadScores(gpgsId, leaderboardStart, rowCount, LeaderboardCollection.Public, leaderboardTimeSpan, data =>
        {
            onloaded?.Invoke(data.Status == ResponseStatus.Success, data);
        });
    }
}
