using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSliders : MonoBehaviour
{
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
        //Ư�� ������ ���� ������ ���� �߰�
        weightSlider.value = value;
    }

    public void ConnectToMonkeyWeight(MonkeyController monkey)
    {
        monkey.weightEvent -= SetValue;
        monkey.weightEvent += SetValue;
        Debug.Log("weight �̺�Ʈ �Ҵ��");
    }


}
