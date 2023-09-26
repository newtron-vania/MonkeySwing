using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AssetKits.ParticleImage;
public class PlayerCoinText : MonoBehaviour
{
    private TextMeshProUGUI _bananaCounttext;
    [SerializeField]
    private ParticleImage _particle;


    private void Start()
    {
        _bananaCounttext = GetComponent<TextMeshProUGUI>();
        StartParticle();
    }
    private void Update()
    {
        _bananaCounttext.text = GameManagerEx.Instance.currentCoin.ToString();
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
        _particle.rateOverTime = Mathf.Min((GameManagerEx.Instance.player.Money - GameManagerEx.Instance.currentCoin) * 2, 50*10);
        _particle.gameObject.SetActive(true);
        _particle.Play();
    }

    public void ParticleOnEvent()
    {
        GameManagerEx.Instance.currentCoin += 5;
    }

    public void ParticleFinishEvent()
    {
        GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.Money;
        _particle.gameObject.SetActive(false);
    }
}
