using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeScene : BaseScene
{
    public override Define.SceneType _sceneType { get { return Define.SceneType.Home; } }
    [SerializeField]
    GameObject cutSceneUI;
    [SerializeField]
    GameObject startToPlayUI;



    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

}
