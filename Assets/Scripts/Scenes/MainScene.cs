using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainScene : BaseScene
{
    public override Define.SceneType _sceneType { get { return Define.SceneType.MainScene; } }

    public Action<MonkeyController> monkeySetEvent;
    private void Start()
    {
        GameManagerEx.Instance.distance = GameObject.FindFirstObjectByType<Distance>();
        GameManagerEx.Instance.makeLines = GameObject.FindFirstObjectByType<MakeLines>();
        StartCoroutine(FindMonkey());
        BananaCount.bananacount = 0;
        GameManagerEx.Instance.GameStart();
        Managers.Sound.Play("MainBGM", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator FindMonkey()
    {
        while (GameObject.FindWithTag("Monkey") == null)
        {
            yield return new WaitForFixedUpdate();
        }
        GameManagerEx.Instance.monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
        monkeySetEvent.Invoke(GameManagerEx.Instance.monkey);
        //Debug.Log($"{this.transform.name} find {monkey.transform.name}");
    }
}
