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
        HomeBGM,
        MainBGM,
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

    public enum Items
    {
        CaloryBanana,
        Boost,
        Magnet,
        Count,
        None
    }
}
