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

    static void Init()
    {
        if (g_instance == null)
        {
            g_instance = new GameManagerEx();
            //TODO
            g_instance.distance = GameObject.FindFirstObjectByType<Distance>();
            g_instance.makeLines = GameObject.FindFirstObjectByType<MakeLines>();
            g_instance.monkey = GameObject.FindFirstObjectByType<MonkeyController>();
        }
    }

    public void GameOver()
    {
        GameStop();
        Managers.Sound.Play("GameOver");
        GameObject.FindWithTag("ScorePopup").SetActive(true);
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
