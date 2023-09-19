using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AssetKits.ParticleImage;
public class PlayerCoinText : MonoBehaviour
{
    TextMeshProUGUI BananaCounttext;
    [SerializeField]
    ParticleImage particle;

    private int diff;

    private void Start()
    {
        BananaCounttext = GetComponent<TextMeshProUGUI>();
        StartParticle();
    }
    private void Update()
    {
        BananaCounttext.text = GameManagerEx.Instance.currentCoin.ToString();
    }

    private void StartParticle()
    {
        if (GameManagerEx.Instance.currentCoin < GameManagerEx.Instance.player.Money)
            StartCoroutine(GetBananaParticle());
    }

    IEnumerator GetBananaParticle()
    {
        yield return null;
        Debug.Log("coin get start");
        particle.rateOverTime = Mathf.Min((GameManagerEx.Instance.player.Money - GameManagerEx.Instance.currentCoin) * 2, 50*10);
        particle.gameObject.SetActive(true);
        particle.Play();
    }

    public void ParticleOnEvent()
    {
        GameManagerEx.Instance.currentCoin += 5;
    }

    public void ParticleFinishEvent()
    {
        GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.Money;
        particle.gameObject.SetActive(false);
    }
}