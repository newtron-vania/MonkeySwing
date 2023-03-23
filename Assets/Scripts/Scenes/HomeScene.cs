using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScene : BaseScene
{
    public override Define.SceneType _sceneType { get { return Define.SceneType.Home; } }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", 0);
            PlayerPrefs.SetInt("MonkeySkinId", 0);
        }

        GameManagerEx.Instance.player.SetData();
        if(BananaCount.bananacount > 0)
        {
            GameManagerEx.Instance.player.Money += BananaCount.bananacount;
            BananaCount.bananacount = 0;
        }

    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
