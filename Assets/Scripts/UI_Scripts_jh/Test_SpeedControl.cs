using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Test_SpeedControl : MonoBehaviour
{
    PlayerController controller;
    [SerializeField]
    TextMeshProUGUI speedText;
    private void Start()
    {
        controller = GameObject.FindAnyObjectByType<PlayerController>();
        setSpeedValue();
    }
    public void SpeedUp()
    {
        controller.slideSpeed += 1f;
        setSpeedValue();
    }

    // Update is called once per frame
    public void SpeedDown()
    {
        controller.slideSpeed -= 1f;
        setSpeedValue();
    }

    private void setSpeedValue()
    {
        speedText.text = ((int)controller.slideSpeed).ToString();
        PlayerPrefs.SetFloat("slideSpeed", controller.slideSpeed);
    }

}
