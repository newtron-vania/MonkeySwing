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
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log(Social.localUser.id + "\n" + Social.localUser.userName);
            if (!success)
            {

            GPGSUIText.text = "Fail Login";
            }
            else
            {
                successAction.Invoke();
            GPGSUIText.text = "Login Succeed";
            }
            hideUIAction.Invoke();
        });
    }


    private void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null)
        {
            GPGSUIText.text =  "No Data saved to the cloud";
            GameManagerEx.Instance.player = new PlayerData();
            return;
        }
        //TODO : 
        string progress = BytesToString(cloudData);
        //json File
        loadedData = progress;
    }


    public void LoadFromCloud(Action<string> afterLoadAction)
    {
        hideUI.SetActive(true);
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
        GPGSUIText.text = "Loading game progress from the cloud.";


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
            isProcessing = true;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToSave);
        }
        else
        {
            Login(() => SaveToCloud(dataToSave), () => hideUI.SetActive(false));
        }
    }

    private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = StringToBytes(loadedData);

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
        }
        else
        {
            GPGSUIText.text = "Error opening Saved Game" + status;
        }
    }


    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
        }
        else
        {
            GPGSUIText.text = "Error opening Saved Game" + status;
        }
    }


    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            GPGSUIText.text = "Error Saving" + status;
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
            GPGSUIText.text =  "Error Saving" + status;
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
}
