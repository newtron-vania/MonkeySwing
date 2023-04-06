using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distance : MonoBehaviour
{
    [SerializeField]
    int dist = 0;
    float bestScore;
    [SerializeField]
    BackgroundScrolling backgroundController;

    [SerializeField]
    TextMeshProUGUI bestScoreText;
    [SerializeField]
    TextMeshProUGUI curScoreText;

    bool isReach = false;
    private void Start()
    {
        bestScore = GameManagerEx.Instance.player.BestScore;
    }
    public int Dist { 
        get { return dist; }
        set
        {
            dist = value;
            SetDist(dist);
        }
    }

    void SetDist(int dist)
    {
        curScoreText.text = dist.ToString() + "m";
        ControlSpeedwithDist();

        if (!isReach)
        {
            if (dist > bestScore)
            {
                isReach = true;
                bestScoreText.color = Color.yellow;
                bestScoreText.fontSize = 100;
            }
        }
        else
        {
            bestScoreText.text = curScoreText.text;
        }
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
