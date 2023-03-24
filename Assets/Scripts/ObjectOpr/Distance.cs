using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distance : MonoBehaviour
{
<<<<<<< HEAD:Assets/Scripts/ObjectOpr/Distance.cs
    [SerializeField]
    int dist = 0;
    [SerializeField]
    BackgroundScrolling backgroundController;
=======
    public static int dist = 0;
>>>>>>> GameOver_Popup:Assets/Scripts/Distance.cs

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
