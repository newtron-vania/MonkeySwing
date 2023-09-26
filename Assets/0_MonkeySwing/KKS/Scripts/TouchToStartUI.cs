using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _monkeyBody;
    [SerializeField]
    private SpriteRenderer _monkeyTale;
    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("HomeBGM", true, Define.Sound.Bgm);
        
        if (GameManagerEx.Instance.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            GameManagerEx.Instance.player.LoadData();
            //Test with PlayerData
            GameManagerEx.Instance.isPlaying = true;
        }
    }

}
