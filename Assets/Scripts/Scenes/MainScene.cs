using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    public override Define.SceneType _sceneType { get { return Define.SceneType.MainScene; } }


    private void Start()
    {
        GameManagerEx.Instance.distance = GameObject.FindFirstObjectByType<Distance>();
        GameManagerEx.Instance.makeLines = GameObject.FindFirstObjectByType<MakeLines>();
        StartCoroutine("FindMonkey");
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
        //Debug.Log($"{this.transform.name} find {monkey.transform.name}");
    }
}
