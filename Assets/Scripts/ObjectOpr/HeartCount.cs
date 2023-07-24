using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartCount : MonoBehaviour
{
    private int heartcount = 3;
    public List<Transform> hearts;

    [SerializeField]
    MainScene gameScene;
    public int Heart
    {
        get { return heartcount; }
        set
        {
            heartcount = value;
            for(int i = 0; i < heartcount; i++)
            {
                hearts[i].gameObject.SetActive(true);
            }
            for(int i = heartcount; i < hearts.Count; i++)
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
    


    private void Awake()
    {
        gameScene.monkeySetEvent -= SetHealthEvent;
        gameScene.monkeySetEvent += SetHealthEvent;
    }

    private void SetHealthEvent(MonkeyController monkey)
    {
        monkey.healthEvent -= SetHeart;
        monkey.healthEvent += SetHeart;
    }

    private void SetHeart(int health)
    {
        Heart = health;
    }
}