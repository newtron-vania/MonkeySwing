using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;
using LitJson;
using System;
using System.Collections.Generic;
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
    public GameObject Skin_content;

    private GameObject Origin_Slot;

    
    void Awake()
    {
        LoadData();
        Origin_Slot = Resources.Load<GameObject>("Prefabs/Shop/Slot");
        Create_TotalSlot();
    }

    public void LoadData(){
        string filename = "Json/SkinDatas";

        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        MySkinList = JsonUtility.FromJson<SkinDatas>(textAsset.text);
        Debug.Log("data load");


        // Application.persistentDataPath + "/" + filename; 
    }

    public void Create_TotalSlot(){
        for (int i=0; i < MySkinList.skins.Count; i++){
            GameObject Slot = Instantiate(Origin_Slot);
            SlotData_Manager slotData_manager = Slot.GetComponent<SlotData_Manager>();
            slotData_manager.Skin_id = MySkinList.skins[i].id;
            Slot.transform.SetParent(Skin_content.transform);
        }  
    }

    public void SaveData()
    {
        string filename = "Json/SkinDatas";

        // 직렬화
        string json = JsonUtility.ToJson(MySkinList, true);
        
        // 파일에 쓰기
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(path, json);

        Debug.Log("data saved");
    }
}
