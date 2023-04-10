using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSliders : MonoBehaviour
{


    [SerializeField]
    Sprite[] FaceStyle;
    [SerializeField]
    Image faceImg;
    [SerializeField]
    Image slideView;
    [SerializeField]
    MainScene GameScene;

    Slider weightSlider;
    private void Start()
    {
        weightSlider = GetComponent<Slider>();
        GameScene.monkeySetEvent -= ConnectToMonkeyWeight;
        GameScene.monkeySetEvent += ConnectToMonkeyWeight;
    }


    public void SetValue(int value)
    {
        //특정 구간을 지날 때마다 사운드 추가
        slideView.fillAmount = (value - 10) / 90f;
        weightSlider.value = value;
        ChangeStateFace(value);
    }

    private void ChangeStateFace(int value)
    {
        if (value <= 30)
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Hunger];
        else if (value <= 80)
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Normal];
        else
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Full];

    } 

    public void ConnectToMonkeyWeight(MonkeyController monkey)
    {
        monkey.weightEvent -= SetValue;
        monkey.weightEvent += SetValue;
        Debug.Log("weight 이벤트 할당됨");
    }


}
