using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image BtnIcon;
    [SerializeField] private string keystring; // 이번엔 이거 EffectVolumn or BGMVolumn
    public Sprite OnImg, OffImg;
    private Define.Sound type;

    void OnEnable()
    {
        if(!PlayerPrefs.HasKey(keystring))
        {
            PlayerPrefs.SetFloat(keystring, 1);
            PlayerPrefs.SetFloat(keystring + "_Pre", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    private void Update() {
        ChangeImg();
    }

    public void SoundBtn_clicked(){
        if (slider.value == 0){
            slider.value = PlayerPrefs.GetFloat(keystring + "_Pre");;
        }
        else if (slider.value != 0){
            PlayerPrefs.SetFloat(keystring + "_Pre", slider.value);
            slider.value = 0;
        }
        SoundSlider_change();
        ChangeImg();
    }

    public void ChangeImg(){
        if (slider.value == 0){
            BtnIcon.sprite = OffImg;
        }
        else if (slider.value != 0){
            BtnIcon.sprite = OnImg; 
        }
    }

    public void SoundSlider_change()
    {
        Managers.Sound.SetAudioVolumn(type, slider.value);
        Save();
    }

    private void Load()
    {
        if (keystring == "BGMVolumn"){
            type = Define.Sound.Bgm;
        }
        else if (keystring == "EffectVolumn"){
            type = Define.Sound.Effect;
        }
        slider.value = PlayerPrefs.GetFloat(keystring);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(keystring, slider.value);
    }
}
