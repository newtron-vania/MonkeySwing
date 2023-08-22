using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSliders : MonoBehaviour
{
    enum ColorSlide
    {
        Blue,
        Yellow,
        Red,
    }

    [SerializeField]
    Sprite[] FaceStyle;
    [SerializeField]
    Image faceImg;
    [SerializeField]
    Image slideView;
    [SerializeField]
    Sprite[] slideColor;
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
        slideView.fillAmount = (float)(value - GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[0]) / (GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[3] - GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[0]);
        weightSlider.value = value;
        ChangeStates(value);
    }

    private void ChangeStates(int value)
    {
        if (value <= GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[1])
        {
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Hunger];
            slideView.sprite = slideColor[(int)ColorSlide.Blue];
        }
        else if (value <= GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[2])
        {
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Normal];
            slideView.sprite = slideColor[(int)ColorSlide.Yellow];
        }
        else
        {
            faceImg.sprite = FaceStyle[(int)Define.CharacterState.Full];
            slideView.sprite = slideColor[(int)ColorSlide.Red];
        }
            

    } 

    public void ConnectToMonkeyWeight(MonkeyController monkey)
    {
        monkey.weightEvent -= SetValue;
        monkey.weightEvent += SetValue;
        monkey.weightEvent.Invoke(monkey.Weight);
        Debug.Log("weight 이벤트 할당됨");
    }


}
