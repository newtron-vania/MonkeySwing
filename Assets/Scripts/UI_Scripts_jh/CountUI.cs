using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI countText;

    // Start is called before the first frame update

    public void SetCount(float time)
    {
        StartCoroutine(countCoroutine(time));
    }

    IEnumerator countCoroutine(float time)
    {
        while(time > 0)
        {
            countText.text = ((int)time).ToString();
            yield return new WaitForSecondsRealtime(1f);
            time--;
        }
        GameManagerEx.Instance.monkey.Health = 3;
        GameManagerEx.Instance.GameStart();
        Managers.Sound.Play("MainBGM", Define.Sound.Bgm);
        this.gameObject.SetActive(false);
    }

}
