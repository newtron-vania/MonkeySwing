using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("HomeBGM", Define.Sound.Bgm);
        
        if (GameManagerEx.Instance.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            GooglePlayManager.Instance.LoadFromCloud((data) => { GameManagerEx.Instance.player = JsonUtility.FromJson<PlayerData>(data); });
            GameManagerEx.Instance.isPlaying = true;
        }
    }
}
