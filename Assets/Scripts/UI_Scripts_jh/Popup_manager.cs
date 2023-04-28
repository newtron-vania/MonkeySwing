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

    public void OnClick_Purchase_Yes_Btn()
    { 
        GameObject clicked_slot_object = SkinData_Manager.clicked_slot;
        SlotData_Manager slotData_manager = clicked_slot_object.GetComponent<SlotData_Manager>();
        OnClickCloseButton();
        slotData_manager.OnClick_Purchase_Yes_Btn();
    }

    public void OnClick_Purchase_No_Btn()
    {
        GameObject current_slot_object = SkinData_Manager.current_slot;
        SlotData_Manager slotData_manager = current_slot_object.GetComponent<SlotData_Manager>();
        slotData_manager.OnClick_Purchase_No_Btn();
        OnClickCloseButton();
    }

    public void ResultPopupOpen()
    {   
        SetTimeScale(0);
        OnClickCloseButton();
    }

    public void OnClickStartButton()
    {   
        SetTimeScale(1);
        // OnClickCloseButton();
        // æ¿ ¿Ã∏ß πŸ≤Ó∏È πŸ≤„æﬂ«‘
        // SceneManager.LoadScene("PlayerMoveTestScene2");
        LoadingScene.LoadScene(Define.SceneType.MainScene);
    }

    public void OnClickHomeButton()
    {
        OnClickCloseButton();
        LoadingScene.LoadScene(Define.SceneType.Home);
    }

    public void OnClickGameOver()
    {
        AdmobManager.Instance.ShowFrontAdWithClick();
        LoadingScene.LoadScene(Define.SceneType.Home);
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
        Managers.Sound.StopPlayingSound(Define.Sound.Effect);
        gameObject.SetActive(false);
        countUI.gameObject.SetActive(true);
        countUI.SetCount(3f);

        HeartCount.is_retry = true;
    }

    public void OnClickReplayButton()
    {
        SetTimeScale(1);
        AddBanana();
        LoadingScene.LoadScene(Define.SceneType.MainScene);
    }

    public void AddBanana()
    {
        GameManagerEx.Instance.player.Money += BananaCount.bananacount;
        BananaCount.bananacount = 0;
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
