using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField]
    MainScene gameScene;
    private byte _backgroundState = 0;

    [SerializeField]
    private Sprite[] _backgroundSprite;

    private int _backgroundNum= 0;

    [SerializeField]
    private float changeBackgroundLimit = 600f;

    private void Start()
    {
        gameScene.distanceSetEvent -= ChangeBackground;
        gameScene.distanceSetEvent += ChangeBackground;
    }

    public void ChangeToNextState()
    {
        _backgroundState += 1;
        SetBackgroundNum();
    }

    private void SetBackgroundNum()
    {
        switch (_backgroundState)
        {
            case 0:
                _backgroundNum = 0;
                break;
            case 1:
                _backgroundNum = 1;
                break;
            case 2:
                _backgroundNum = 2;
                break;
            case 3:
                _backgroundNum = 4;
                break;
        }
    }

    public void SetBackgroundImg(SpriteRenderer background)
    {
        background.sprite = _backgroundSprite[_backgroundNum];
        switch (_backgroundState)
        {
            case 0:
                ChangeToNextState();
                break;
            case 2:
                if (_backgroundNum++ >= 3)
                    ChangeToNextState();
                break;
            case 3:
                _backgroundNum = _backgroundNum >= 5 ? 4 : _backgroundNum + 1 ;
                break;
        }
    }

    private void ChangeBackground(int value)
    {
        if(value>0 && value == changeBackgroundLimit)
        {
            ChangeToNextState();
        }
    }
}
