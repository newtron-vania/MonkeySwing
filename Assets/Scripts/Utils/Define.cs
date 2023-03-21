using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Monkey,
        Enemy
    }

    public enum PopupUIGroup
    {
        Unknown
    }

    public enum SceneUI
    {
        Unknown
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum BGMs
    {
        BGM_01,
        BGM_02,
        BGM_03,
        BGM_04,
        A_Bit_Of_Hope
    }
    public enum UIEvent
    {
        Click,
        Drag,

    }
    public enum SceneType
    {
        Unknown,
        Home,
        LoadingScene,
        MainScene,
        GameScene
    }
}
