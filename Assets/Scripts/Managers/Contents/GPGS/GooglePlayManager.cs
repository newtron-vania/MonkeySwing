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

public class GooglePlayManager : MonoBehaviour
{
    static private GooglePlayManager instance;
    static public GooglePlayManager Instance
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

    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@GooglePlayManager");
            if (go == null)
            {
                go = new GameObject { name = "@GooglePlayManager" };
                go.AddComponent<GooglePlayManager>();
                hideUI = Managers.Resource.Instantiate("UI/HideUI");
                hideUI.SetActive(false);
            }
            DontDestroyOnLoad(go);
            DontDestroyOnLoad(hideUI);
            instance = go.GetComponent<GooglePlayManager>();
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
    public void Login(Action successAction, Action hideUIAction)
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
            StartCoroutine(TryFirebaseLogin(successAction, hideUIAction));
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
    IEnumerator TryFirebaseLogin(Action successAction, Action hideUIAction)
    {
        Debug.Log("Gooo");
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        Debug.Log($"firebase idtoken - " + idToken);

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        Debug.Log($"credential - " + credential);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithCredentialAsync was canceled!!");
                hideUIAction.Invoke();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithCredentialAsync encountered an error: " + task.Exception);
                hideUIAction.Invoke();
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            FireBaseId = newUser.UserId;

            Debug.Log("Success!");
            Debug.Log($"FireBaseId : " + FireBaseId);
            Debug.Log("firebase Success!!");

            database = FirebaseDatabase.DefaultInstance.GetReference("users");

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
            Login(() => StartCoroutine(LoadFromCloudRoutin(afterLoadAction)), () => hideUI.SetActive(false));
        }
    }

    private IEnumerator LoadFromCloudRoutin(Action<string> loadAction)
    {
        isProcessing = true;
        Debug.Log("Loading game progress from the cloud.");
        hideUI.SetActive(true);
        //Test
        FireBaseId = "abcde";
        database.Child(FireBaseId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
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
            database.Child(FireBaseId).SetRawJsonValueAsync(loadedData).ContinueWithOnMainThread(task => 
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
            Login(() => SaveToCloud(dataToSave), () => hideUI.SetActive(false));
        }
    }


    public void LoadBestScoreRankingArray(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null)
    {
        hideUI.SetActive(true);
        database.OrderByChild("bestscore").LimitToFirst(rowCount).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            List<UserRankData> userRankDatas = new List<UserRankData>();
            bool success = false;
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception}");
                success = false;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int i = 0;
                foreach(DataSnapshot data in snapshot.Children)
                {
                    IDictionary snap = (IDictionary)data.Value;
                    UserRankData userData = new UserRankData() { userName = snap["username"].ToString(), skinID = (int)snap["currentskinid"], bestScore = (long)snap["bestscore"], rank = i+1 };
                    userRankDatas.Add(userData);
                }
                success = true;
            }

            onLoadedRankAction.Invoke(success, userRankDatas);
            // Do something with snapshot...
        });
    }
}

public class UserRankData
{
    public string userName;
    public int skinID;
    public long bestScore;
    public int rank;

}
