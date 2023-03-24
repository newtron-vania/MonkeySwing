using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distance : MonoBehaviour
{
    [SerializeField]
    int dist = 0;
    [SerializeField]
    BackgroundScrolling backgroundController;

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
        GetComponent<TextMeshProUGUI>().text = dist.ToString() + "m";
    }

    void ControlSpeedwithDist()
    {
        if(Dist>0 && Dist%100 == 0)
        {
            GameManagerEx.Instance.makeLines.LineSpeed += 0.3f;
            backgroundController.IsDay = !backgroundController.IsDay;
            Debug.Log("Dist Start!");
        }
    }
}
