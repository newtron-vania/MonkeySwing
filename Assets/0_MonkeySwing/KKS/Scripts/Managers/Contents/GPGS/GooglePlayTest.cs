using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooglePlayTest : MonoBehaviour
{
    private void OnGUI()
    {
        int x = 0;
        int y = 0;
        if (GUI.Button(new Rect(x, y, 150, 100), "UseFirebase"))
        {
            GooglePlayManager.Instance.UseFirebase();
            GooglePlayManager.Instance.FireBaseID = "Dx4C0mjiQmWChtba22EoJFHrX6z1";
        }
        y += 100;

        if (GUI.Button(new Rect(x, y, 150, 100), "LoadUser"))
        {
            GooglePlayManager.Instance.LoadUser(loadData =>
            {
                PlayerData player = JsonUtility.FromJson<PlayerData>(loadData);
                Debug.Log($"userName : {player.UserName}\n currentskinid : {player.MonkeySkinId} \n money : {player.Money}");
            },
            "Dx4C0mjiQmWChtba22EoJFHrX6z1"
            );
        }
        y += 100;
        if (GUI.Button(new Rect(x, y, 150, 100), "LoadScore"))
        {
            GooglePlayManager.Instance.LoadScore(loadData =>
            {
                ScoreData scoreData = new ScoreData();

                Debug.Log($"LoadScore Start! : {loadData}");
                scoreData.BestScore = DictionaryJsonUtility.FromJson<string, int>(loadData);
                
                foreach(var d in scoreData.BestScore)
                {
                    Debug.Log($"{d.Key} : {d.Value}");
                }
            },
            "Dx4C0mjiQmWChtba22EoJFHrX6z1"
            );
        }
        y += 100;
        if (GUI.Button(new Rect(x, y, 150, 100), "SaveScore"))
        {
            ScoreData data = new ScoreData();
            data.SetScore(1, 300);
            data.SetScore(2, 35);
            data.ShowScore();
            GooglePlayManager.Instance.SaveScore(DictionaryJsonUtility.ToJson(data.BestScore, false));
        }
        y += 100;
        if (GUI.Button(new Rect(x, y, 150, 100), "ShowRanking"))
        {

            GooglePlayManager.Instance.LoadBestScoreRankingArray(10,
                (sucess, sender) =>
                {
                    Debug.Log($"rank count : {sender.Count}");
                    foreach (UserRankData data in sender)
                    {
                        Debug.Log($"name : {data.userName} skin : {data.skinID} score : {data.bestScore} rank : {data.rank}");
                    }
                }
            );
        }
        y += 100;
    }
}
