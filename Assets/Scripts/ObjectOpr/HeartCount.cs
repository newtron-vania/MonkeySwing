using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartCount : MonoBehaviour
{
    public static int heartcount = 3;
    public GameObject[] hearts;
    private static bool[] flag = new bool[3] {true, true, true};   
    public static bool is_retry = false;
    public GameObject resultPopup;

    void OnEnable()
    {
        heartcount = 3;
        heartImg_reset();
    }

    void Update()
    {   
        // GameObject.Find("HeartCount").GetComponent<TextMeshProUGUI>().text = "heartcount : " + heartcount.ToString();

        if (heartcount != 3 && flag[heartcount])
        {
            GameObject ht = hearts[heartcount];
            ht.SetActive(false);
            flag[heartcount] = false;
        }

        if (is_retry)
        {
            heartImg_reset();
        }
    }

    
    public void heartImg_reset()
    {
        if (heartcount == 3 && flag[heartcount-1] == false)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject.Find("HeartContainer").transform.GetChild(i).gameObject.SetActive(true);
                flag[heartcount-(i+1)] = true;
            }
        }
    } 
}