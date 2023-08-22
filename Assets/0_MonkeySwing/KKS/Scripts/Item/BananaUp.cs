using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaUp : MonoBehaviour
{
    public float maxTTL = 10f;
    private float time = 0f;

    private void FixedUpdate()
    {
        if (time >= maxTTL)
            Managers.Resource.Destroy(this.gameObject);
        transform.rotation = Quaternion.identity;
        time += Time.fixedDeltaTime;
    }
    private void SetItemStat()
    {

    }

    public void ResetTime()
    {
        BananaCount.bananaCountUpEvent -= AddBanana;
        BananaCount.bananaCountUpEvent += AddBanana;
        time = 0f;
    }

    private void AddBanana()
    {
        BananaCount.bananacount += 0.1f;
    }

    private void OnEnable()
    {
        SetItemStat();
        ResetTime();
    }
    private void OnDestroy()
    {
        BananaCount.bananaCountUpEvent -= AddBanana;
        Debug.Log($"Destory BananaUp in {time}");
    }
}
