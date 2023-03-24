using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("HomeBGM", Define.Sound.Bgm);
        if(GameManagerEx.Instance.isPlaying)
        {
            Debug.Log($"bananacount : {BananaCount.bananacount}");
            GameManagerEx.Instance.player.Money += BananaCount.bananacount;
            Debug.Log($"player.Money : {GameManagerEx.Instance.player.Money}");
            BananaCount.bananacount = 0;
            this.gameObject.SetActive(false);
        }
        else
        {
            GameManagerEx.Instance.isPlaying = true;
        }
    }
}
