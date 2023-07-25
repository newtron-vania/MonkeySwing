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
    private Sprite BodySprite;
    private Sprite TailSprite;
    private SkinData_Manager skinData_manager = new SkinData_Manager();

<<<<<<< HEAD
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
=======
    
    private void Awake()
>>>>>>> Shop
    {
        LoadData();
        Origin_Slot = Resources.Load<GameObject>("Prefabs/Shop/Slot");
        Create_TotalSlot();
        Update_skindata();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData(){
        string filename = "Json/SkinDatas";

        TextAsset textAsset = Resources.Load<TextAsset>(filename);
        MySkinList = JsonUtility.FromJson<SkinDatas>(textAsset.text);
        Debug.Log("data load");
<<<<<<< HEAD

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
=======
>>>>>>> Shop
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

    public void Update_skindata()
    {
        // playerdata와 비교해서 locked된 스킨, 현재 스킨 data update
        GameManagerEx.Instance.player.AddSkinId(0); // basic skin은 자동 추가
        for (int count=0; count < MySkinList.skins.Count; count++)
        {
            SkinData now_Skindata = MySkinList.skins[count];
            // playerSkin과 previewSkin 모두 저장되어 있는 skin으로 변경
            if (now_Skindata.id ==  GameManagerEx.Instance.player.MonkeySkinId){
                now_Skindata.is_current_PlayerSkin = true;
                now_Skindata.is_current_PreviewSkin = true;
                SkinImg_Load(now_Skindata);
                skinData_manager.MonkeyPrefabSKin_Change(BodySprite, TailSprite, now_Skindata);
                skinData_manager.MonkeyPreviewSKin_Change(BodySprite, TailSprite, now_Skindata);
            }
            if ( GameManagerEx.Instance.player.GetSkinIds().Contains(now_Skindata.id))
            {
                now_Skindata.is_locked = false;
            }
        }
    }

    public void SkinImg_Load(SkinData skindata)
    {
        string body_path = skindata.monkey_skin_path[0];
        string tail_path = skindata.monkey_skin_path[1];
        BodySprite = Resources.Load<Sprite>(body_path);
        TailSprite = Resources.Load<Sprite>(tail_path);
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
