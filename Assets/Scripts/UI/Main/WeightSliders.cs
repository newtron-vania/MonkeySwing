using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightSliders : MonoBehaviour
{
    Slider weightSlider;

    private void Start()
    {
        weightSlider = GetComponent<Slider>();
    }


    public void SetValue(int value)
    {
        weightSlider.value = value;
    }
}
