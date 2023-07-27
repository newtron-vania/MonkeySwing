using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer monkeyBody;
    [SerializeField]
    SpriteRenderer monkeyTale;
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
            GameManagerEx.Instance.isPlaying = true;
        }
        Debug.Log($"BestScore : {GameManagerEx.Instance.player.BestScore}");
    }

}
