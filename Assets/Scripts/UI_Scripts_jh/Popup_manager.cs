using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Popup_manager : MonoBehaviour
{
    ChangeScene changeScene = new ChangeScene();
    [SerializeField]
    CountUI countUI;
    private void OnEnable()
    {
        SetTimeScale(0);
        //Debug.Log("Time.timeScale0" + Time.timeScale);
    }

    private void SetTimeScale(int time){
        Time.timeScale = time;
    }

    public void ResultPopupOpen()
    {   
        SetTimeScale(0);
        gameObject.SetActive(true);
        //OnClickCloseButton();
    }

    public void OnClickStartButton()
    {   
        SetTimeScale(1);
        // OnClickCloseButton();
        // æ¿ ¿Ã∏ß πŸ≤Ó∏È πŸ≤„æﬂ«‘
        // SceneManager.LoadScene("PlayerMoveTestScene2");
        changeScene.ChangeSceneBtn("MainScene");
    }

    public void OnClickHomeButton()
    {
        OnClickCloseButton();
        BananaCount.bananacount = 0;
        changeScene.ChangeSceneBtn("Home");
    }

    public void OnClickGameOverButton()
    {
        OnClickCloseButton();
        changeScene.ChangeSceneBtn("Home");
    }

    public void OnClickContinueButton()
    {
        OnClickCloseButton();
    }

    public void OnClickCloseButton()
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        //animator.SetTrigger("close");
        yield return null;
        SetTimeScale(1);
        gameObject.SetActive(false);
        //Debug.Log("Time.timeScale1" + Time.timeScale);
        //animator.ResetTrigger("close");
    }

    public void OnClickRetryButton()
    {
        gameObject.SetActive(false);
        countUI.gameObject.SetActive(true);
        countUI.SetCount(3f);

        HeartCount.is_retry = true;
    }

    public void OnClickExitButton()
    {
        Debug.Log("click Exit");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   
        SetTimeScale(0);
        Application.Quit();
#endif
    }
}
