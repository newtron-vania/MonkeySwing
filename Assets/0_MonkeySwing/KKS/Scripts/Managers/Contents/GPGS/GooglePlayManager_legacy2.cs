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
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Linq;
using System.Text.RegularExpressions;

public class GooglePlayManager_legacy2 : MonoBehaviour
{
    static private GooglePlayManager_legacy2 instance;
    static public GooglePlayManager_legacy2 Instance
    {
        get { Init(); return instance; }
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

    public bool isAuthenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    public string LocalUser
    {
        get
        {
            if (string.IsNullOrEmpty(Social.localUser.userName))
                return "melon";
            return Social.localUser.userName;
        }
    }

    public bool isFirebaseAuth
    {
        get
        {
            return FireBaseId == null ? true : false;
        }
    }

    private FirebaseAuth auth;
    private string FireBaseId = string.Empty;
    DatabaseReference database;

    string userDataName = "users";
    string userScoreName = "ranks";

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@GooglePlayManager");
            if (go == null)
            {
                go = new GameObject { name = "@GooglePlayManager" };
                go.AddComponent<GooglePlayManager_legacy2>();
                hideUI = Managers.Resource.Instantiate("UI/HideUI");
                hideUI.SetActive(false);
            }
            DontDestroyOnLoad(go);
            DontDestroyOnLoad(hideUI);
            instance = go.GetComponent<GooglePlayManager_legacy2>();
        }
    }

    private void InitiatePlayGames()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .EnableSavedGames() // Saved Games 기능 활성화
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        auth = FirebaseAuth.DefaultInstance;
    }

    private void Awake()
    {
        InitiatePlayGames();
    }
    public void Login(Action successAction)
    {
        hideUI.SetActive(true);
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log($"Login Check {Social.localUser.id + "\n" + Social.localUser.userName}");
            if (!success)
            {
                Debug.Log("Fail Login");
            }
            else
            {
                //AdmobManager.Instance.call();
                Debug.Log("Login Succeed");
            }
            StartCoroutine(TryFirebaseLogin(successAction));
        });
    }

    public void TryGoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
    }


    public void TryFirebase()
    {
        database = FirebaseDatabase.DefaultInstance.GetReference("users");
        StartCoroutine(LoadFromCloudRoutin(null));

    }
    IEnumerator TryFirebaseLogin(Action successAction)
    {
        Debug.Log("Gooo");
        float time = 3f;
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
            time -= Time.unscaledTime;
            if (time < 0f)
                break;
        }
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        Debug.Log($"firebase idtoken - " + idToken);

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        Debug.Log($"credential - " + credential);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            Debug.Log("SignInWithCredentialAsync start");
            database = FirebaseDatabase.DefaultInstance.RootReference;
            hideUI.SetActive(false);
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithCredentialAsync was canceled!!");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            FireBaseId = newUser.UserId;

            Debug.Log("Success!");
            Debug.Log($"FireBaseId : " + FireBaseId);
            Debug.Log("firebase Success!!");

            

            successAction.Invoke();
        });
    }

    public void LoadFromCloud(Action<string> afterLoadAction)
    {

        if (isAuthenticated && !isProcessing)
        {
            StartCoroutine(LoadFromCloudRoutin(afterLoadAction));
        }
        else
        {
            Login(() => StartCoroutine(LoadFromCloudRoutin(afterLoadAction))) ;
        }
    }

    private IEnumerator LoadFromCloudRoutin(Action<string> loadAction, string id = null)
    {
        isProcessing = true;
        Debug.Log("Loading game progress from the cloud.");
        hideUI.SetActive(true);
        //Test
        if (id == null) id = FireBaseId;
        database.Child(userDataName).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception.ToString()}");
                loadedData = JsonUtility.ToJson(new PlayerData());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Key);
                Debug.Log(snapshot.GetRawJsonValue());
                loadedData = snapshot.GetRawJsonValue();
                if (string.IsNullOrEmpty(loadedData))
                {
                    loadedData = JsonUtility.ToJson(new PlayerData());
                }
                loadedData = Regex.Unescape(loadedData);
                Debug.Log(loadedData);
            }
            isProcessing = false;
            // Do something with snapshot...
        });

        while (isProcessing)
        {
            yield return null;
        }
        loadAction.Invoke(loadedData);
        hideUI.SetActive(false);
        //loadAction.Invoke(loadedData);
    }

    

    public void SaveToCloud(string dataToSave)
    {
        if (isAuthenticated)
        {
            loadedData = dataToSave;
            loadedData = Regex.Unescape(loadedData);
            Debug.Log("loadedData");
            isProcessing = true;

            database.Child(userDataName).Child(FireBaseId).SetRawJsonValueAsync(loadedData).ContinueWithOnMainThread(task => 
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log("Firebase Save Error!");
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Firebase Save Complete");
                    Debug.Log(loadedData);
                }
                isProcessing = false;
            });
        }
        else
        {
            Login(() => SaveToCloud(dataToSave));
        }
    }


    public void LoadBestScoreRankingArray(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        hideUI.SetActive(true);
        database.Child(userScoreName).OrderByChild(mapid.ToString()).LimitToFirst(rowCount).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("bestScore is in");
            List<UserRankData> userRankDatas = new List<UserRankData>();
            bool success = false;
            if (task.IsFaulted || task.IsCanceled)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception}");
                success = false;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log($"rankingLoad is Complete! {snapshot.GetRawJsonValue()}");
                int i = 0;
                foreach(DataSnapshot data in snapshot.Children)
                {
                    Debug.Log($"rank {i} , {data.GetRawJsonValue()}, {JsonUtility.ToJson((IDictionary)data.Value)}");
                    PlayerData snap = JsonUtility.FromJson<PlayerData>(data.GetRawJsonValue());
                    UserRankData userData = new UserRankData() { userName = snap.UserName, skinID = snap.MonkeySkinId, bestScore = snap.BestScore, rank = ++i };
                    userRankDatas.Add(userData);
                }
                success = true;
            }
            Debug.Log("RankLoad is Over");
            hideUI.SetActive(false);
            Debug.Log("Rank HideUI False");
            onLoadedRankAction.Invoke(success, userRankDatas);
            // Do something with snapshot...
        });
    }
    public void LoadBestScoreRankingArray_Proto(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        hideUI.SetActive(true);
        database.Child(userScoreName).OrderByChild(mapid.ToString()).LimitToFirst(rowCount).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("bestScore is in");
            UserRankData[] userRankDatas = new UserRankData[rowCount];
            bool success = false;
            if (task.IsFaulted || task.IsCanceled)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception}");
                success = false;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log($"rankingLoad is Complete! {snapshot.GetRawJsonValue()}");
                int i = 0;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    Debug.Log($"rank {i} , {data.GetRawJsonValue()}, {JsonUtility.ToJson((IDictionary)data.Value)}");
                    ScoreData snap = JsonUtility.FromJson<ScoreData>(data.GetRawJsonValue());
                    string userId = data.Key;
                    StartCoroutine(LoadFromFirebaseRoutin_Proto((loadData) =>
                   {
                       PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadData);
                       UserRankData rankData = new UserRankData()
                       {
                           userName = playerData.UserName,
                           skinID = playerData.MonkeySkinId,
                           bestScore = snap.GetScore(mapid),
                           rank = ++i
                       };
                       userRankDatas[rankData.rank-1] = rankData; 
                   }, 
                    userId));
                }
                success = true;
            }
            Debug.Log("RankLoad is Over");
            hideUI.SetActive(false);
            Debug.Log("Rank HideUI False");
            onLoadedRankAction.Invoke(success, userRankDatas.ToList());
            // Do something with snapshot...
        });
    }

    private IEnumerator LoadFromFirebaseRoutin_Proto(Action<string> loadAction, string id = null)
    {
        string loadedData = string.Empty;
        isProcessing = true;
        Debug.Log("Loading game progress from the cloud.");
        hideUI.SetActive(true);
        //Test
        if (id == null) id = FireBaseId;
        database.Child(userDataName).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception.ToString()}");
                loadedData = JsonUtility.ToJson(new PlayerData());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Key);
                Debug.Log(snapshot.GetRawJsonValue());
                loadedData = snapshot.GetRawJsonValue();
                if (string.IsNullOrEmpty(loadedData))
                {
                    loadedData = JsonUtility.ToJson(new PlayerData());
                }
                loadedData = Regex.Unescape(loadedData);
                Debug.Log(loadedData);
            }
            isProcessing = false;
            // Do something with snapshot...
        });

        while (isProcessing)
        {
            yield return null;
        }

        loadAction.Invoke(loadedData);
        hideUI.SetActive(false);
        //loadAction.Invoke(loadedData);
    }
}

