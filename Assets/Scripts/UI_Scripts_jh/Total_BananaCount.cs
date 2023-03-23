using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Total_BananaCount : MonoBehaviour
{

    private void Awake() {
        PlayerPrefs.SetInt("totalbananacount", PlayerPrefs.GetInt("totalbananacount", 0));
    }

    void Start()
    {
        
    }

    void Update()
    {
        /*
        int now_banana = PlayerPrefs.GetInt("totalbananacount");
        GetComponent<TextMeshProUGUI>().text = now_banana.ToString();
        */
    }
}

