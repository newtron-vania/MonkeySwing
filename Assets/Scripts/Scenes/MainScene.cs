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
        GameManagerEx.Instance.monkey = GameObject.FindFirstObjectByType<MonkeyController>();
        Managers.Sound.Play("MainBGM", Define.Sound.Bgm);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
