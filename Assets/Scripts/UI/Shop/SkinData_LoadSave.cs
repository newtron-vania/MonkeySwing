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
    public GameObject Skin_content;

    private GameObject Origin_Slot;

    [SerializeField]
    private SkinData_Manager skinDataManager;

    [SerializeField]
    private SpriteRenderer Monkey_prefab_body;
    [SerializeField]
    private SpriteRenderer Monkey_prefab_tail;


    private void Awake()
    {
        Init();
    }

    public void Init()
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

        List<int> skinlist = GameManagerEx.Instance.player.GetSkinIds();
        for (int i = 0; i < MySkinList.skins.Count; i++)
        {
            if (skinlist.Contains(MySkinList.skins[i].id))
            {
                MySkinList.skins[i].is_locked = false;
            }
            if (MySkinList.skins[i].id == GameManagerEx.Instance.player.MonkeySkinId)
            {
                MySkinList.skins[i].is_current_PlayerSkin = true;
                MySkinList.skins[i].is_current_PreviewSkin = true;
                Monkey_prefab_body.sprite = Resources.Load<Sprite>(MySkinList.skins[i].monkey_skin_path[0]);
                Monkey_prefab_tail.sprite = Resources.Load<Sprite>(MySkinList.skins[i].monkey_skin_path[1]);
            }
        }
        // Application.persistentDataPath + "/" + filename; 
    }

    public void Create_TotalSlot(){
        for (int i=0; i < MySkinList.skins.Count; i++){
            GameObject Slot = Instantiate(Origin_Slot, Skin_content.transform);
            SlotData_Manager slotData_manager = Slot.GetComponent<SlotData_Manager>();
            slotData_manager.Skin_id = MySkinList.skins[i].id;
            slotData_manager.init(skinDataManager);
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
