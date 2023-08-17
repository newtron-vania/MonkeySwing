using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData
{
    [SerializeField]
    private Dictionary<string, int> bestScore = new Dictionary<string, int>();
    public int GetScore(int stageID)
    {
        if (!bestScore.ContainsKey(stageID.ToString()))
            bestScore.Add(stageID.ToString(), 0);
        return bestScore[stageID.ToString()];
    }

    public void SetScore(int stageID, int score)
    {
        if (!bestScore.ContainsKey(stageID.ToString()))
            bestScore.Add(stageID.ToString(), 0);
        bestScore[stageID.ToString()] = score;
    }
}