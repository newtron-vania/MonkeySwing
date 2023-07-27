using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinState : MonoBehaviour, State
{
    public GameObject Content, MonkeyIcon;
    public Image TabImage;

    public string SkinStateName;

    public void On()
    {
        Content.SetActive(true);
        MonkeyIcon.SetActive(true);
        TabImage.color = Color.gray;
    }

    public void Off()
    {
        Content.SetActive(false);
        MonkeyIcon.SetActive(false);
        TabImage.color = Color.white;
    }
}
