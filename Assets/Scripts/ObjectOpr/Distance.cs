using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    public Action<int> distanceEvent;

    bool isReach = false;
    private void Start()
    {
        bestScore = GameManagerEx.Instance.player.BestScore;
        bestScoreText.text = bestScore.ToString() + "m";
    }

    public int Dist { 
        get { return dist; }
        set
        {
            dist = value;
            SetDist(dist);
            distanceEvent.Invoke(dist);
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
                curScoreText.color = Color.yellow;
                curScoreText.fontSize = 120;
                bestScoreText.fontSize = 100;
                bestScoreText.text = curScoreText.text;
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
