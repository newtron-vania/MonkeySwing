using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distance : MonoBehaviour
{
    public static int distance = 0;

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Distance : " + distance.ToString() + "m";
    }
}
