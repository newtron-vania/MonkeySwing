using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeartCount : MonoBehaviour
{
    public static int heartcount = 3;
 
    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Heart Count : " + heartcount.ToString();
        //Debug.Log(heartcount);
    }
}
