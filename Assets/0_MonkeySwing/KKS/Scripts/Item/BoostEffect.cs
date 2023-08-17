using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostEffect : MonoBehaviour
{
    public float boostForce = 3f;

    [SerializeField]
    Image BoostSlider;

    float TTL = 4f;
    float waitInvincibleTime = 2f;
    float curTime = 0f;
    private void SetItemStat()
    {
        ItemDataSO itemStat = Managers.Data.GetItem("Booster");
        TTL = itemStat.time;
        boostForce = itemStat.value;
    }
    void OnEnable()
    {
        SetItemStat();
        Managers.Sound.Play("FeverBGM", Define.Sound.Bgm);
    }

    private void Update()
    {
        if (curTime > TTL)
        {
            Managers.Sound.Play("MainBGM", Define.Sound.Bgm);
            Managers.Resource.Destroy(this.gameObject);
        }
        curTime += Time.deltaTime;
        BoostSlider.fillAmount = (TTL - curTime) * (1 / TTL);
        Debug.Log($"Boost gage : {(TTL - curTime) * (1 / TTL)}");
    }

    public void ResetTime()
    {
        Managers.Sound.Play("Boost");
        curTime = 0f;
        GameManagerEx.Instance.monkey.StartBoost(TTL, waitInvincibleTime);
        GameManagerEx.Instance.makeLines.BoostLineSpeed(TTL, boostForce);
    }
}
