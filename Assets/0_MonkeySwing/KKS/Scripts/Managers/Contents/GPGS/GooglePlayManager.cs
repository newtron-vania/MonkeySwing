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
    public string FireBaseID { get { return FireBaseId; } set { FireBaseId = value; } }
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
    public void Login(Action successAction)
    {
        hideUI.SetActive(true);
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log($"Login Check {Social.localUser.id + "\n" + Social.localUser.userName}");
            if (!success)
            {
                Debug.Log("Fail Login");
                GameManagerEx.Instance.player = new PlayerData();
                hideUI.SetActive(false);
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

    public void UseFirebase()
    {
        
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    IEnumerator TryFirebaseLogin(Action callback)
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
            //hideUI.SetActive(false);
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

            

            callback.Invoke();
        });
    }

    public void LoadFromCloud(Action<string> callbackPlayer, Action<string> callbackScore)
    {

        if (isAuthenticated && !isProcessing)
        {
            LoadUser(callbackPlayer);
            LoadScore(callbackScore);
        }
        else
        {
            Login(() => { LoadUser(callbackPlayer); LoadScore(callbackScore); });
        }
    }

    public void LoadUser(Action<string> callback, string id = null)
    {
        isProcessing = true;
        hideUI.SetActive(true);
        //Test
        if (id == null) id = FireBaseId;
        Debug.Log($"id : {id}");
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
            callback.Invoke(loadedData);
            hideUI.SetActive(false);
        });
    }

    public void SaveUser(string dataToSave)
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
            Login(() => SaveUser(dataToSave));
        }
    }
    public void SaveUser(string tokenid, string dataToSave)
    {
        if (isAuthenticated || database != null)
        {
            loadedData = dataToSave;
            loadedData = Regex.Unescape(loadedData);
            Debug.Log("loadedData");
            isProcessing = true;

            database.Child(userDataName).Child(tokenid).SetRawJsonValueAsync(loadedData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log("Firebase Save Error!");
                    Debug.Log(task.Exception.ToString());
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
            Login(() => SaveUser(dataToSave));
        }
    }

    public void LoadScore(Action<string> callback, string id = null)
    {
        isProcessing = true;
        hideUI.SetActive(true);
        //Test
        if (id == null) id = FireBaseId;
        database.Child(userScoreName).Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Handle the error...
                Debug.Log($"Firebase DataLoad Error. {task.Exception.ToString()}");
                loadedData = JsonUtility.ToJson(new ScoreData());
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Key);
                Debug.Log(snapshot.GetRawJsonValue());
                loadedData = snapshot.GetRawJsonValue();
                if (string.IsNullOrEmpty(loadedData))
                {
                    loadedData = JsonUtility.ToJson(new ScoreData());
                }
                loadedData = Regex.Unescape(loadedData);
                Debug.Log(loadedData);
            }
            isProcessing = false;
            // Do something with snapshot...
            callback.Invoke(loadedData);
            hideUI.SetActive(false);
        });
    }

    public void SaveScore(string dataToSave)
    {
        if (isAuthenticated || true)
        {
            loadedData = dataToSave;
            loadedData = Regex.Unescape(loadedData);
            Debug.Log(loadedData);
            isProcessing = true;

            database.Child(userScoreName).Child(FireBaseId).SetRawJsonValueAsync(loadedData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log($"Firebase Save Error!, {task.Exception.ToString()}");
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
            Login(() => SaveUser(dataToSave));
        }
    }

    public void LoadBestScoreRankingArray(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        StartCoroutine(LoadBestScoreRankingCoroutine(rowCount, onLoadedRankAction, mapid));
    }
    public void LoadBestScoreRankingArray2(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        StartCoroutine(LoadBestScoreRankingCoroutine2(rowCount, onLoadedRankAction, mapid));
    }
    public IEnumerator LoadBestScoreRankingCoroutine(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        bool success = false;
        UserRankData[] userRankDatas = new UserRankData[rowCount];

        hideUI.SetActive(true);
        database.Child(userScoreName).OrderByChild($"data/{(mapid-1).ToString()}/Value").LimitToFirst(rowCount).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("bestScore is in");
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
                    Dictionary<string, int> snap = DictionaryJsonUtility.FromJson<string, int>(data.GetRawJsonValue());
                    string userId = data.Key;
                    LoadUser((loadData) =>
                   {
                       PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadData);
                       UserRankData rankData = new UserRankData()
                       {
                           userName = playerData.UserName,
                           skinID = playerData.MonkeySkinId,
                           bestScore = -1 * snap[mapid.ToString()],
                           rank = ++i
                       };
                       userRankDatas[rankData.rank-1] = rankData;

                       if (i >= snapshot.ChildrenCount)
                       {
                           success = true;
                           Array.Resize(ref userRankDatas, i);
                       }
                           
                   }, 
                    userId);
                }
            }
        });

        float time = 2f;
        while (!success && time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }

        Debug.Log("RankLoad is Over");
        hideUI.SetActive(false);
        Debug.Log("Rank HideUI False");
        onLoadedRankAction.Invoke(success, userRankDatas.ToList());
        // Do something with snapshot...
    }

    public IEnumerator LoadBestScoreRankingCoroutine2(int rowCount, Action<bool, List<UserRankData>> onLoadedRankAction = null, int mapid = 1)
    {
        bool success = false;
        List<UserRankData> userRankDatas = new List<UserRankData>();

        hideUI.SetActive(true);
        database.Child(userScoreName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("bestScore is in");
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
                    Debug.Log($"{data.GetRawJsonValue()}, {JsonUtility.ToJson((IDictionary)data.Value)}");
                    Dictionary<string, int> snap = DictionaryJsonUtility.FromJson<string, int>(data.GetRawJsonValue());
                    string userId = data.Key;
                    LoadUser((loadData) =>
                    {
                        PlayerData playerData = JsonUtility.FromJson<PlayerData>(loadData);
                        UserRankData rankData = new UserRankData()
                        {
                            userName = playerData.UserName,
                            skinID = playerData.MonkeySkinId,
                            bestScore = -1 * snap[mapid.ToString()],
                            rank = 0
                        };
                        userRankDatas.Add(rankData);
                        i++;
                        if (i >= snapshot.ChildrenCount)
                        {
                            success = true;
                        }
                    },
                    userId);
                }
            }
        });

        float time = 2f;
        while (!success && time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        userRankDatas.Sort((a, b) => a.bestScore > b.bestScore ? -1 : 1);
        int count = userRankDatas.Count > rowCount ? rowCount : userRankDatas.Count;
        for (int i = 1; i <= count; i++)
        {
            userRankDatas[i-1].rank = i;
        }
        Debug.Log("RankLoad is Over");
        hideUI.SetActive(false);
        Debug.Log("Rank HideUI False");
        onLoadedRankAction.Invoke(success, userRankDatas.GetRange(0, count));
        // Do something with snapshot...
    }

}

public class UserRankData
{
    public string userName;
    public int skinID;
    public long bestScore;
    public int rank;

}
