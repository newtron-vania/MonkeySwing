using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData
{
    [SerializeField]
    private Dictionary<string, int> bestScore = new Dictionary<string, int>();

    public Dictionary<string, int> BestScore { get { return bestScore; } set { bestScore = value; } }

    public int GetScore(int stageID)
    {
        while(!bestScore.ContainsKey(stageID.ToString()) )
            bestScore.Add(stageID.ToString(), 0);
        return -1 * bestScore[stageID.ToString()];
    }

    public void SetScore(int stageID, int score)
    {
        while (!bestScore.ContainsKey(stageID.ToString()))
            bestScore.Add(stageID.ToString(), 0);
        bestScore[stageID.ToString()] = -1 * score;
        GooglePlayManager.Instance.SaveScore(DictionaryJsonUtility.ToJson(GameManagerEx.Instance.scoreData.BestScore, false));
    }

    public void ShowScore()
    {
        foreach(var score in bestScore)
        {
            Debug.Log($"map : {score.Key}, score : {score.Value}");
        }
    }
}