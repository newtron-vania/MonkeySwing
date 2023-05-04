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


    public enum CharacterState
    {
        Hunger,
        Normal,
        Full,
        Damaged,
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
        FeverBGM,
        ShopBGM
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

    public enum Rarelity
    {
        Normal,
        Rare,
        Unique,
        Legendary
    }

}
