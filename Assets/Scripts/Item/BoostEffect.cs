using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    public float boostForce = 3f;

    float TTL = 4f;
    float waitInvincibleTime = 2f;
    float curTime = 0f;
    void Start()
    {
        GameManagerEx.Instance.monkey.StartBoost(TTL, waitInvincibleTime);
        GameManagerEx.Instance.makeLines.BoostLineSpeed(TTL, boostForce);
        Managers.Sound.Play("Boost");
    }

    private void Update()
    {
        if (curTime > TTL)
        {
            Managers.Resource.Destroy(this.gameObject);
        }
        curTime += Time.deltaTime;
    }

    public void ResetTime()
    {
        Managers.Sound.Play("Boost");
        curTime = 0f;
        GameManagerEx.Instance.monkey.StartBoost(TTL, waitInvincibleTime);
        GameManagerEx.Instance.makeLines.BoostLineSpeed(TTL, boostForce);
    }
}
