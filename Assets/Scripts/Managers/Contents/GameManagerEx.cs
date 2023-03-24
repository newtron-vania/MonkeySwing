using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    static GameManagerEx g_instance;
    public static GameManagerEx Instance { get { Init(); return g_instance; } }

    public PlayerData player;
    public MakeLines makeLines;
    public Distance distance;
    public MonkeyController monkey;
    public BananaCount banana;
    public bool isPlaying = false;

    static void Init()
    {
        if (g_instance == null)
        {
            Debug.Log("Instance Reset");
            g_instance = new GameManagerEx();
            //TODO
            g_instance.distance = GameObject.FindFirstObjectByType<Distance>();
            g_instance.makeLines = GameObject.FindFirstObjectByType<MakeLines>();
            g_instance.monkey = GameObject.FindFirstObjectByType<MonkeyController>();
            g_instance.player = new PlayerData();
        }
    }


    public void GameOver()
    {
        GameManagerEx.Instance.GameStop();
        Managers.Sound.Clear();
        Managers.Sound.Play("GameOver");
        GameObject resultPopup = GameObject.FindObjectOfType<HeartCount>().resultPopup;
        resultPopup.SetActive(true);
        //UI ���� �� ó��
    }
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
