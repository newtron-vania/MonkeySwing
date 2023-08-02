using System.Collections;
using AssetKits.ParticleImage;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParticleText : MonoBehaviour
{

    [SerializeField]
    ParticleImage particle;

    [SerializeField]
    Text text;

    public int fnum = 100;

    public int nNum = 100;

    public int lnum = 200;


    private void OnEnable()
    {
        particle.rateOverTime = 1000;
        StartCoroutine(SetEnable());
    }

    IEnumerator SetEnable()
    {
        yield return null;
        if (!particle.isPlaying)
        {
            Debug.Log("onenalbe Play");
            particle.timeScale = AssetKits.ParticleImage.Enumerations.TimeScale.Unscaled;
            particle.Play();
            Debug.Log(particle.isPlaying);
        }
    }
    private void Start()
    {
        nNum = fnum;
        particle.gameObject.SetActive(true);

    }

    private void Update()
    {
        text.text = nNum.ToString();
    }

    public void ParticleStart()
    {
        nNum = fnum;
    }
    public void ParticleEvent()
    {
        nNum += 1;
    }
    public void ParticleEnd()
    {
        nNum = lnum;
    }
}
