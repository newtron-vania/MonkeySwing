using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.IO;
using LitJson;
using System;


//public struct SkinData
//[System.Serializable]
//public class SkinData 
public struct SkinData{
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


public class Monkey_Skin : MonoBehaviour
{
    
    public int id;
    private Sprite BodySprite;
    private Sprite TailSprite;

    SkinData currentSkinData = new SkinData();
    JsonData itemData;
    // test 용으로 data 직접 집어넣기, 원래는 자동화
    /*public SkinData currentSkinData = new SkinData {
            id = 0,
            name = "Monkey_blue",
            is_locked = true,
            slot_lock_img_path = "Shop/Slot/box_yellow_locked",
            slot_unlock_img_path = "Shop/Slot/box_yellow",
            monkey_skin_path = new List<string> { "Shop/Skin/Monkey_yellow", "Shop/Skin/Monkey_yellow_tail" },
            price = 100,
            is_current_PlayerSkin = false,
            is_current_PreviewSkin = false,
        };*/

    public void Data_Load()
    {
        Debug.Log("불러오기");
        string filePath = Application.dataPath + "/example.json";
        
        string jsonData = File.ReadAllText(filePath);
        Debug.Log(jsonData);

        itemData = JsonMapper.ToObject(jsonData);
        Debug.Log(itemData);
        Debug.Log(itemData[id]["name"].GetType().Name);
        Debug.Log(itemData[id]["name"].ToString().GetType().Name);
        Debug.Log(itemData[id]["is_current_PlayerSkin"]);
        Debug.Log(itemData[id]["is_current_PlayerSkin"].GetType().Name);
       // Debug.Log(JSON.parse("false").GetType().Name);

        JsonData nowData = itemData[id];

        currentSkinData.name = nowData["name"].ToString();
        currentSkinData.is_locked = Convert.ToBoolean(nowData["is_locked"].ToString());
        currentSkinData.slot_lock_img_path = nowData["slot_lock_img_path"].ToString();
        currentSkinData.slot_unlock_img_path =  nowData["slot_unlock_img_path"].ToString();
        currentSkinData.monkey_skin_path = new List<string> { nowData["monkey_skin_path"][0].ToString(), nowData["monkey_skin_path"][1].ToString() };
        currentSkinData.price = Convert.ToInt32(nowData["price"].ToString());
        currentSkinData.is_current_PlayerSkin = Convert.ToBoolean(nowData["is_current_PlayerSkin"].ToString());
        currentSkinData.is_current_PreviewSkin = Convert.ToBoolean(nowData["is_current_PreviewSkin"].ToString());



        // JSON 데이터를 C# 객체로 역직렬화
        //List<SkinData> itemData1 = new List<SkinData>();
        //List<SkinData> itemData1 = JsonUtility.FromJson<List<SkinData>>(jsonData);
       // Debug.Log(itemData1[0]);

        // currentSkinData = itemData[0];

    }



    public void SkinImg_Load()
    {
        string body_path = currentSkinData.monkey_skin_path[0];
        string tail_path = currentSkinData.monkey_skin_path[1];
        BodySprite = Resources.Load<Sprite>(body_path);
        TailSprite = Resources.Load<Sprite>(tail_path);
    }

    public void PreviewBtn_Img_Load()
    {
        string PreviewBtn_path = "";
        if(currentSkinData.is_locked){
            PreviewBtn_path = currentSkinData.slot_lock_img_path;
        }
        else if(!currentSkinData.is_locked){
            PreviewBtn_path = currentSkinData.slot_unlock_img_path;
        }

        Sprite PreviewBtn_Img = Resources.Load<Sprite>(PreviewBtn_path);
        GameObject PreviewBtn = transform.Find("Skin_preview_btn").gameObject;
        PreviewBtn.GetComponent<Image>().sprite = PreviewBtn_Img;
    }

    public void PurchaseBtn_Load()
    {
        string state = "";
        if(currentSkinData.is_locked){ //잠겨있으면
            state = currentSkinData.price.ToString();
        }
        else if(!currentSkinData.is_locked){ //안잠겨있고
            if(currentSkinData.is_current_PlayerSkin){ // 현재 착용중이면
                state = "사용중";
            }
            else{
                state = "장착";
            }
        }
        
        GameObject PurchaseBtn = GameObject.Find("state").gameObject;
        PurchaseBtn.GetComponent<TextMeshProUGUI>().text = state;

    }

    void Awake() {
        
    }

    void Start()
    {
        Data_Load();
        SkinImg_Load();
        PreviewBtn_Img_Load();
        PurchaseBtn_Load();
        
    }

    public void OnClick_purchase_Btn()
    {
        Reset_is_current_PlayerSkin();
        if (!currentSkinData.is_locked){
            //구매하시겠습니까 팝업 띄우기

        }
        else if(currentSkinData.is_locked)
        {
            if(currentSkinData.is_current_PlayerSkin)
            {
                Debug.Log("이미 장착중인 스킨입니다.");
            }
            if(!currentSkinData.is_current_PlayerSkin)
            {
                MonkeyPreviewSKin_Change();
                MonkeyPrefabSKin_Change();
                // slot 파란 박스선 추가
                PurchaseBtn_Load();
            }

        }
    }

    public void OnClick_Preview_Btn()
    {
        Reset_is_current_PreviewSkin();
        if (!currentSkinData.is_current_PreviewSkin)
        {
            MonkeyPreviewSKin_Change();
            // Preview Monkey name change
            GameObject MonkeyName = GameObject.Find("MonkeyName_TXT").gameObject;
            MonkeyName.GetComponent<TextMeshProUGUI>().text = currentSkinData.name; // 계속 저장해야 함.!!!
        }
    }

    public void MonkeyPreviewSKin_Change()
    {
        GameObject Monkey_preview = GameObject.Find("Monkey_preview");

        GameObject Monkey_body = Monkey_preview.transform.Find("Monkey_Body").gameObject;
        Monkey_body.GetComponent<Image>().sprite = BodySprite;
        GameObject Monkey_tail = Monkey_preview.transform.Find("Monkey_Tail").gameObject;
        Monkey_tail.GetComponent<Image>().sprite = TailSprite;

        string path = "Assets/Resources/Shop/Monkey_preview.prefab";
        PrefabUtility.SaveAsPrefabAsset(Monkey_preview, path);
        currentSkinData.is_current_PreviewSkin = true;
    }

    public void MonkeyPrefabSKin_Change()
    {
        GameObject Monkey_prefab = GameObject.Find("Monkey_prefab");
        
        GameObject Monkey_body = Monkey_prefab.transform.Find("Monkey_Body").gameObject;
        Monkey_body.GetComponent<SpriteRenderer>().sprite = BodySprite;
        GameObject Monkey_tail = Monkey_prefab.transform.Find("Monkey_Tail").gameObject;
        Monkey_tail.GetComponent<SpriteRenderer>().sprite = TailSprite;

        string path = "Assets/Resources/Shop/Monkey_prefab.prefab";
        PrefabUtility.SaveAsPrefabAsset(Monkey_prefab, path);
        currentSkinData.is_current_PlayerSkin = true;
    }

    public void Reset_is_current_PlayerSkin()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            itemData[i]["is_current_PlayerSkin"] = false;
        }
    }

    public void Reset_is_current_PreviewSkin()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            itemData[i]["is_current_PreviewSkin"] = false;
        }
    }

    
}
