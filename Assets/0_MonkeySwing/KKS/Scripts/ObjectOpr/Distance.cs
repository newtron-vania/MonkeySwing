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
        bestScore = GameManagerEx.Instance.scoreData.GetScore(GameManagerEx.Instance.mapID);
        bestScoreText.text = "BEST  " + bestScore.ToString() + "m";
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

        if (!isReach)
        {
            if (dist > bestScore)
            {
                isReach = true;
                curScoreText.color = Color.yellow;
                curScoreText.fontSize = 120;
                bestScoreText.fontSize = 80;
                bestScoreText.text = "BEST " + curScoreText.text;
            }
        }

        else
        {
            bestScoreText.text = curScoreText.text;
        }
    }
}
