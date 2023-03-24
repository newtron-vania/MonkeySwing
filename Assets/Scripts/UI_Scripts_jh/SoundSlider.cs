using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider EffectSlider;

    void OnEnable()
    {
        if(!PlayerPrefs.HasKey("BGMVolumn"))
        {
            PlayerPrefs.SetFloat("BGMVolumn", 1);
            PlayerPrefs.SetFloat("EffectVolumn", 1);
            Load();
        }
        else
        {
            Load();
        }
    }


    public void ChangeBGM()
    {
        Managers.Sound.SetAudioVolumn(Define.Sound.Bgm, BGMSlider.value);
        Save();
    }
    
    public void ChangeEffect()
    {
        Managers.Sound.SetAudioVolumn(Define.Sound.Effect, EffectSlider.value);
        Save();
    }

    private void Load()
    {
        BGMSlider.value = PlayerPrefs.GetFloat("BGMVolumn");
        EffectSlider.value = PlayerPrefs.GetFloat("EffectVolumn");

    }

    private void Save()
    {
        PlayerPrefs.SetFloat("BGMVolumn", BGMSlider.value);
        PlayerPrefs.SetFloat("EffectVolumn", EffectSlider.value);

    }
}
