using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainScene : BaseScene
{
    [SerializeField]
    Distance distance;
    [SerializeField]
    MakeLines makeline;
    public override Define.SceneType _sceneType { get { return Define.SceneType.MainScene; } }

    public Action<MonkeyController> monkeySetEvent;
    public Action<int> distanceSetEvent;
    private void Awake()
    {
        Managers.Data.CreateItemDict();
        GameManagerEx.Instance.distance = distance;
        GameManagerEx.Instance.distance.distanceEvent -= StartDistanceEvent;
        GameManagerEx.Instance.distance.distanceEvent += StartDistanceEvent;
        

        GameManagerEx.Instance.makeLines = makeline;
        StartCoroutine(FindMonkey());
        BananaCount.bananacount = 0;

        GameManagerEx.Instance.itemManager.Init();

        GameManagerEx.Instance.GameStart();
        Managers.Sound.Play("MainBGM", Define.Sound.Bgm);
    }

    public void StartDistanceEvent(int score)
    {
        distanceSetEvent.Invoke(score);
    }

    public override void Clear()
    {
        GameManagerEx.Instance.distance = null;
        GameManagerEx.Instance.makeLines = null;
        GameManagerEx.Instance.monkey = null;
    }

    IEnumerator FindMonkey()
    {
        while (GameObject.FindWithTag("Monkey") == null)
        {
            yield return new WaitForFixedUpdate();
        }
        GameManagerEx.Instance.monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
        monkeySetEvent.Invoke(GameManagerEx.Instance.monkey);

        transform.GetComponent<StartItem>().Init();
        //Debug.Log($"{this.transform.name} find {monkey.transform.name}");
    }
}
