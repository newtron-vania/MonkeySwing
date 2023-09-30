using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaUp : MonoBehaviour
{
    public float maxTTL = 10f;
    private float time = 0f;

    private Transform bananaUpImg;

    private void FixedUpdate()
    {
        if (time >= maxTTL)
            Managers.Resource.Destroy(this.gameObject);
        transform.rotation = Quaternion.identity;
        time += Time.fixedDeltaTime;
    }
    public void ResetTime()
    {
        BananaCount.bananaCountUpEvent -= AddBanana;
        BananaCount.bananaCountUpEvent += AddBanana;
        time = 0f;
    }
    private void ShowItemUsing()
    {
        bananaUpImg = GameObject.FindObjectOfType<IsBananaUI>().transform.Find("bananaUpImg");
        bananaUpImg.gameObject.SetActive(true);
    }

    private void ShowItemDisable()
    {
        if(bananaUpImg == null)
        {
            bananaUpImg = GameObject.FindObjectOfType<IsBananaUI>().transform.Find("bananaUpImg");
        }
        bananaUpImg.gameObject.SetActive(true);
    }
    private void AddBanana()
    {
        BananaCount.bananacount += 0.1f;
    }

    private void OnEnable()
    {
        ShowItemUsing();
        ResetTime();
    }
    private void OnDestroy()
    {
        ShowItemDisable();
        BananaCount.bananaCountUpEvent -= AddBanana;
        Debug.Log($"Destory BananaUp in {time}");
    }
}
