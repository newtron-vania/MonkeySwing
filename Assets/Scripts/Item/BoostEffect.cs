using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MonoBehaviour
{
    public float boostForce = 2f;
    MonkeyController monkey;
    MakeLines lineGenerator;

    float TTL = 8f;
    float waitInvincibleTime = 2f;
    float curTime = 0f;
    void Start()
    {
        monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
        monkey.StartBoost(TTL, waitInvincibleTime);
        lineGenerator = GameManagerEx.Instance.makeLines;
        lineGenerator.BoostLineSpeed(TTL, boostForce);
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
        monkey.StartBoost(TTL, waitInvincibleTime);
        lineGenerator.BoostLineSpeed(TTL, boostForce);
    }
}
