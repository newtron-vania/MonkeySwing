using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GPGSDataSaveTest : MonoBehaviour
{
    string log;
    private void Start()
    {
        GPGSBinder.Inst.LoginCheck((success, localUser) =>
            log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");



        CallPlayerData();
    }
    private void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 3);


        if (GUILayout.Button("SavePlayerData"))
            SavePlayerData();

        GUILayout.Label(log);
    }


    void SavePlayerData()
    {
        PlayerData player = CallPlayerData();
        string data = JsonUtility.ToJson(player);
        GPGSBinder.Inst.SaveCloud("playerData", data, success => log = $"{success}");

    }

    PlayerData CallPlayerData()
    {
        PlayerData player = new PlayerData();
        player.BestScore = 200;
        player.Money = 250;
        player.MonkeySkinId = 3;

        return player;
    }
}
