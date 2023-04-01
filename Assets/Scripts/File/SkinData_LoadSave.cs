using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;
using LitJson;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SkinData
{
    public int id;
    public string name;
    public bool is_locked;
    public string slot_lock_img_path;
    public string slot_unlock_img_path;
    public List<string> monkey_skin_path;
    public int price;
    public bool is_current_PlayerSkin;
    public bool is_current_PreviewSkin;
}

[System.Serializable]
public class SkinDatas {
    public List<SkinData> skins;
}

public class SkinData_LoadSave : MonoBehaviour
{
    public static SkinDatas MySkinList = new SkinDatas();
    void Awake()
    {
        string filename = "Json/SkinDatas";

        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        MySkinList = JsonUtility.FromJson<SkinDatas>(textAsset.text);
        Debug.Log("data load");
    }
}