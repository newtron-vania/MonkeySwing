using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum State
	{
		Die,
		Moving,
		Idle,
		Skill
    }
public enum Layer
    {
        Ground = 8,
        Block = 9,
        Monster = 10,
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }

}
