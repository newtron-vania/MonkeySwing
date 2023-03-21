using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distance : MonoBehaviour
{
    int dist = 0;

    public int Dist { 
        get { return dist; }
        set
        {
            dist = value;
            ControlSpeedwithDist();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "Distance : " + dist.ToString() + "m";
    }

    void ControlSpeedwithDist()
    {
        if(Dist>0 && Dist%100 == 0)
        {
            GameManagerEx.Instance.makeLines.LineSpeed += 0.3f;
        }
    }
}
