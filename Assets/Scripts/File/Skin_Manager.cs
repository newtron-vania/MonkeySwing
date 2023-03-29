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
 

//public struct SkinData
//[System.Serializable]
//public class SkinData 


public class Skin_Manager : MonoBehaviour
{
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

     public class SkinDatas
    {
        public SkinData[] skins;
    }


    public int id;

    private string filename = "SkinDatas.json";
    private Sprite BodySprite;
    private Sprite TailSprite;

    SkinDatas MySkinList = new SkinDatas();

    JsonData Skins = new JsonData ();
    SkinData currentSkinData = new SkinData();

    //JsonData Skins_ = new jsonData();
    //static List<SkinData> Skins = new List<SkinData> ();
    //JsonData currentSkinData 
   
   void Awake() {
        //FileHandler.SaveToJSON<SkinData> (Skins, filename);
        Data_Load();
    }

    void Start()
    {
        SkinImg_Load();
        PreviewBtn_Img_Load();
        PurchaseBtn_Load();        
    }

    public void Data_Load()
    {
        Debug.Log("불러오기");
        // List<SkinData>로 json파일의 내용 받아옴.
        string filePath = Application.dataPath + "/example.json";
        
        string jsonData = File.ReadAllText(filePath);
        Debug.Log(jsonData);

        MySkinList = JsonUtility.FromJson<SkinDatas>(jsonData);
        Debug.Log(MySkinList.Count);


        
        Skins = JsonMapper.ToObject(jsonData);

        JsonData nowData = Skins[id];
        currentSkinData = nowData.ToList();


        //string filePath = Application.dataPath + "/SkinDatas.json";
        //string jsonData = File.ReadAllText(filePath);
        //Skins = JsonUtility.FromJson<List<SkinData>>(jsonData);

        // Skins = FileHandler.ReadListFromJSON<SkinData>("SkinDatas.json");
        Debug.Log(Skins.Count);
        Debug.Log(Skins[0]["name"]);  
        // id번째의 skindata를 currentSkinData에 저장.
        //currentSkinData = Skins[id];
        Debug.Log(currentSkinData);
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
        
        GameObject PurchaseBtn = transform.Find("state").gameObject;
        PurchaseBtn.GetComponent<TextMeshProUGUI>().text = state;

    }

    public void OnClick_purchase_Btn()
    {
        Reset_is_current_PlayerSkin();
        if (!currentSkinData.is_locked){
            //구매하시겠습니까 팝업 띄우기
            // 구매 진행

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
        for (int i = 0; i < Skins.Count; i++)
        {
            SkinData temp = Skins[i];
            temp.is_current_PlayerSkin = false;
            Skins[i] = temp;
        }
    }

    public void Reset_is_current_PreviewSkin()
    {
        for (int i = 0; i < Skins.Count; i++)
        {
            SkinData temp = Skins[i];
            temp.is_current_PreviewSkin = false;
            Skins[i] = temp;
        }
    }
}
