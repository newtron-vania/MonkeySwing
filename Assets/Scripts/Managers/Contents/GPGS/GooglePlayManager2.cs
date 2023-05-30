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

public class GooglePlayManager2 : MonoBehaviour
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

    private const string m_saveFileName = "playerData";
    public bool isAuthenticated
    {
        get
        {
            return Social.localUser.authenticated;
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
            hideUIAction.Invoke();
            if (!success)
            {
                Debug.Log("Fail Login");
            }
            else
            {
                StartCoroutine(TryFirebaseLogin(successAction, hideUIAction));
                AdmobManager.Instance.call();
                Debug.Log("Login Succeed");
            }

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

    IEnumerator TryFirebaseLogin(Action successAction, Action hideUIAction)
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        Debug.Log($"firebase idtoken - " + idToken);

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        Debug.Log($"credential - " + credential);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
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

            database = FirebaseDatabase.DefaultInstance.RootReference;

            successAction.Invoke();
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
        else if(!isFirebaseAuth)
        {
            StartCoroutine(TryFirebaseLogin(() => StartCoroutine(LoadFromCloudRoutin(afterLoadAction)), () => hideUI.SetActive(false)));
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



    public void LoadBestScoreRankingArray(int rowCount, LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, UserRankData[]> onLoadedMyRankAction = null, Action<bool, UserRankData[]> onLoadedRankAction = null)
    {
        hideUI.SetActive(true);

        CustomLoadLeaderBoard(GPGSIds.leaderboard_bestscore,
            LeaderboardStart.PlayerCentered,
            1,
            LeaderboardCollection.Public,
            leaderboardTimeSpan,
            onLoadedMyRankAction);
        CustomLoadLeaderBoard(GPGSIds.leaderboard_bestscore,
            LeaderboardStart.TopScores,
            10,
            LeaderboardCollection.Public,
            leaderboardTimeSpan,
            onLoadedRankAction,
            true);
    }

    private void CustomLoadLeaderBoard(string leaderBoardID, LeaderboardStart startPosition, int rowCount, LeaderboardCollection leaderboardCollection, LeaderboardTimeSpan leaderboardTimeSpan, Action<bool, UserRankData[]> onLoadedAction, bool controllhideUI = false)
    {
        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_bestscore,
            startPosition,
            rowCount,
            leaderboardCollection,
            leaderboardTimeSpan,
            data =>
            {
                Debug.Log($"UserScore Load success : {data.Status == ResponseStatus.Success}");
                LoadUsers(data.Status == ResponseStatus.Success, data.Scores, onLoadedAction, controllhideUI);
            });
    }

    private void LoadUsers(bool success, IScore[] scores, Action<bool, UserRankData[]> onloaded = null, bool controllhideUI = false)
    {
        if (success)
        {
            string[] userIds = new string[scores.Length];

            for (int i = 0; i < scores.Length; i++)
            {
                userIds[i] = scores[i].userID;
                Debug.Log($"userIds : {userIds[i]}");
            }
            // forward scores with loaded profiles
            Debug.Log("LoadUser Start");
            Social.LoadUsers(userIds, profiles => loadUserName(profiles, scores, onloaded, controllhideUI));
        }
        else
        {
            onloaded?.Invoke(success, null);
            if (controllhideUI)
                hideUI.SetActive(false);
        }
    }

    private void loadUserName(IUserProfile[] profiles, IScore[] scores, Action<bool, UserRankData[]> onloaded = null, bool controllhideUI = false)
    {
        UserRankData[] userRankDatas = new UserRankData[profiles.Length];
        Debug.Log($"profile length : {profiles.Length}");
        foreach (IUserProfile profile in profiles)
        {
            Debug.Log($"profile id : {profile.id}");
            Debug.Log($"profile id : {profile.userName}");
        }
        for (int i = 0; i < profiles.Length; i++)
        {
            Debug.Log($"userScore : {scores[i].value}");
            Debug.Log($"rank : {scores[i].rank}");
            Debug.Log($"profile name : {profiles[i].userName}");
            userRankDatas[i] = new UserRankData() { userName = profiles[i].userName, userScore = scores[i].value, rank = scores[i].rank };
            Debug.Log($"userdata {i} = {userRankDatas[i].userName}, {userRankDatas[i].userScore}, {userRankDatas[i].rank}");
            Debug.Log($"{i} complete");
        }
        Debug.Log($" userRankData Set Complete!");
        onloaded?.Invoke(true, userRankDatas);

        if (controllhideUI)
            hideUI.SetActive(false);
    }

}
