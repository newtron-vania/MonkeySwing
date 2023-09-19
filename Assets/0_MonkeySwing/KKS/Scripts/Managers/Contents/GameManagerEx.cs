using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    static GameManagerEx g_instance;
    public static GameManagerEx Instance { get { Init(); return g_instance; } }


    public int currentCoin;
    public PlayerData player;
    public ScoreData scoreData;
    public MakeLines makeLines;
    public Distance distance;
    public MonkeyController monkey;
    public BananaCount banana;

    public bool isPlaying = false;

    public int mapID = 1;
    static void Init()
    {
        if (g_instance == null)
        {
            g_instance = new GameManagerEx();
            //TODO
            g_instance.distance = GameObject.FindFirstObjectByType<Distance>();
            g_instance.makeLines = GameObject.FindFirstObjectByType<MakeLines>();
            g_instance.monkey = GameObject.FindFirstObjectByType<MonkeyController>();
            g_instance.player = new PlayerData();
            g_instance.scoreData = new ScoreData();
            g_instance.currentCoin = g_instance.player.Money;
            Debug.Log($"g_instance.player : { g_instance.player.Money}");
        }
    }


    public void GameOver()
    {
        GameManagerEx.Instance.GameStop();
        Managers.Sound.Clear();
        GameObject resultPopup = GameObject.FindObjectOfType<ResultUI>(true).gameObject;
        resultPopup.SetActive(true);

        // Invoke("Delay", 5);

        //Popup_manager popup_mg = new Popup_manager();
        //popup_mg.ResultPopupOpen();

        //StartCoroutine(DelayCoroution());
        //UI ���� �� ó��
    }

    /*IEnumerator DelayCoroution()
    {
        yield return new WaitForSeconds(5);
        GameObject resultPopup = GameObject.FindObjectOfType<HeartCount>().resultPopup;
        resultPopup.SetActive(true);
    }*/
     

    //�ð� ����
    public void GameStop()
    {
        Time.timeScale = 0;
    }


    //�ð� �ٽ� �ǽ�
    public void GameStart()
    {
        Time.timeScale = 1;
    }

}