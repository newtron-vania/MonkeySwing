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

public class FireBaseManager : MonoBehaviour
{
    private FirebaseAuth auth;
    public string FireBaseId = string.Empty;

    public static FireBaseManager Instance = null;

    void Awake()
    {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        auth = FirebaseAuth.DefaultInstance;

        Instance = this;
    }


    public void Start()
    {
        Login();
    }

    public void Login()
    {
        Debug.Log("GameCenter Login");
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    Debug.Log("google game service Success");
                    //SystemMessageManager.Instance.AddMessage("google game service Success");
                    StartCoroutine(TryFirebaseLogin()); // Firebase Login 시도
                }
                else // 실패하면
                {
                    Debug.Log("google game service Fail");
                }
            });
        }
    }


    public void TryGoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
    }


    IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();


        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
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
        });
    }
}
