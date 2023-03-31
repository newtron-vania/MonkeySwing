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


public class Skin_Manager : MonoBehaviour
    {
    public int Skin_id;
    private static int currentSkin_ID;


    private Sprite BodySprite;
    private Sprite TailSprite;

    //SkinData_Manager skindata_MG = new SkinData_Manager();
    //List<SkinData> MySkinList = new List<SkinData>();
    //MySkinList = SkinData_Manager.MySkinList.skins;
    //public SkinData_Manager skindata_MG = new SkinData_Manager();
    // skindata_MG.MySkinList
    SkinData currentSkinData;
   
    void Start()
    {
        Debug.Log("Skin_Manager ����! ID : " +  Skin_id);
        Debug.Log(SkinData_Manager.MySkinList.skins.Count);
        Debug.Log(SkinData_Manager.MySkinList.skins[0].name);

        currentSkinData =  Get_SkinData(Skin_id);
;
        SkinImg_Load();
        PreviewBtn_Img_Load();
        PurchaseBtn_Load();        
    }

    public void CurrentSkinData_Load()
    {
        if (currentSkinData == null){
            currentSkinData = Get_SkinData(currentSkin_ID);
        }
    }

    // ���� Skin_id�� ������ sprite �ҷ�����
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
        CurrentSkinData_Load();
        Debug.Log(currentSkinData + "�� ���� ��Ų ������ �Դϴ�.");
        if(currentSkinData.is_locked){ // ��������� �ݾ� ��������
            state = currentSkinData.price.ToString();
        }
        else if(!currentSkinData.is_locked){ //�����������
            if(currentSkinData.is_current_PlayerSkin){ // ���� �÷��̾� ��Ų�̶��
                state = "�����";
                // ���⼭ ���� �Ķ��ڽ� �߰��ϱ�
            }
            else{
                state = "����"; // ���� �÷��̾� ��Ų�� �ƴ϶��
            }
        }
        GameObject PurchaseBtn = transform.Find("Skin_purchase_btn").gameObject;
        GameObject PurchaseBtn_Txt = PurchaseBtn.transform.Find("state").gameObject;
        PurchaseBtn_Txt.GetComponent<TextMeshProUGUI>().text = state;

    }

    public void OnClick_purchase_Btn()
    {
        currentSkin_ID = Skin_id;
        CurrentSkinData_Load(); // currentSkinData ����ִ��� Ȯ��
        if (currentSkinData.is_locked){
            // ��Ų ���� �˾� ����
            Debug.Log("�����Ͻðڽ��ϱ�");
            GameObject Purchase_popup = GameObject.Find("Shop").transform.Find("Purchase_popup").gameObject;
            Purchase_popup.SetActive(true);
            // popup â���� yes or no �Ǵ�
        }
        else if(!currentSkinData.is_locked)
        {
            if(currentSkinData.is_current_PlayerSkin)
            {
                Debug.Log("�̹� ������� ��Ų�Դϴ�.");
            }
            else if(!currentSkinData.is_current_PlayerSkin)
            {
                Debug.Log("��Ų�� �����մϴ�. ��Ų ������ �Ϸ�Ǿ����ϴ�.");
                Reset_is_current_PlayerSkin();
                currentSkinData.is_current_PlayerSkin = true;
                
                OnClick_Preview_Btn();
                MonkeyPrefabSKin_Change();
                PurchaseBtn_Load();
            }
        }
    }

    public int GetCurrentPreviewID(){
        //Debug.Log(MySkinList.skins[2]);
        Debug.Log(SkinData_Manager.MySkinList.skins);
        Debug.Log(SkinData_Manager.MySkinList.skins[0].name);
        for (int count = 0; count < SkinData_Manager.MySkinList.skins.Count; count++)
        {
            if (SkinData_Manager.MySkinList.skins[count].is_current_PreviewSkin == true){
                return SkinData_Manager.MySkinList.skins[count].id;
            }
        }
        return 0;
    }

    public void OnClick_Purchase_Yes_Btn()
    { 
        //GameManagerEx.Instance.player.Money


        Debug.Log("���Ž����մϴپƾ�");
        int ID = GetCurrentPreviewID();
        CurrentSkinData_Load();
        Debug.Log("�����ϰڽ��ϴ�.");
        SkinData now_Skin = Get_SkinData(ID);
        Debug.Log( now_Skin.price + " : ���� �ٳ���");

        //int bananacount = PlayerPrefs.GetInt("totalbananacount");
       
        GameManagerEx.Instance.player.Money = 10000; // ���߿� �����
        int bananacount = GameManagerEx.Instance.player.Money;
        Debug.Log(bananacount + " : ��ü �ٳ���");
        if (bananacount >= now_Skin.price){
            Debug.Log("��Ų�� �����մϴ�.");
            now_Skin.is_locked = false;
            bananacount -= now_Skin.price;
            GameManagerEx.Instance.player.Money = bananacount; 
            // PlayerPrefs.SetInt("Money", bananacount); // �̰� ������Ѿ� ��ȭ���� ����
            PurchaseBtn_Load(); // ��ư ���¸� �������� �ٲٱ�
        }
        else if (bananacount < now_Skin.price){
             Debug.Log("��ȭ�� �����մϴ�.");
        }
        //gameObject.SetActive(false);
    }

    public void OnClick_Purchase_No_Btn()
    {
        int ID = GetCurrentPreviewID();
        SkinData now_Skin = Get_SkinData(ID);

        now_Skin.is_locked = true;
        Debug.Log("��Ų��  �������� �ʽ��ϴ�.");
        //gameObject.SetActive(false);
    }

    public void OnClick_Preview_Btn()
    {
        // PreviewBtn_Img_Load();
        if (!currentSkinData.is_current_PreviewSkin)
        {
            MonkeyPreviewSKin_Change();
            Reset_is_current_PreviewSkin();
            currentSkinData.is_current_PreviewSkin = true;
        }
    }

    public void MonkeyPreviewSKin_Change()
    {
        GameObject Monkey_preview = GameObject.Find("Monkey_preview");

        GameObject Monkey_body = Monkey_preview.transform.Find("Monkey_Body").gameObject;
        Monkey_body.GetComponent<Image>().sprite = BodySprite;
        GameObject Monkey_tail = Monkey_preview.transform.Find("Monkey_Tail").gameObject;
        Monkey_tail.GetComponent<Image>().sprite = TailSprite;
        GameObject MonkeyName = Monkey_preview.transform.Find("MonkeyName").gameObject;
        GameObject MonkeyName_TXT = MonkeyName.transform.Find("MonkeyName_TXT").gameObject;
        MonkeyName_TXT.GetComponent<TextMeshProUGUI>().text = currentSkinData.name;

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
        for (int count = 0; count < SkinData_Manager.MySkinList.skins.Count; count++)
        {
            if (SkinData_Manager.MySkinList.skins[count].is_current_PlayerSkin == true){
                // �Ķ� �ڽ� ����, �������� �ؽ�Ʈ ����
                SkinData_Manager.MySkinList.skins[count].is_current_PlayerSkin = false;
            }
        }
    }

    public void Reset_is_current_PreviewSkin()
    {
        for (int count = 0; count < SkinData_Manager.MySkinList.skins.Count; count++)
        {
            if (SkinData_Manager.MySkinList.skins[count].is_current_PreviewSkin == true){
                SkinData_Manager.MySkinList.skins[count].is_current_PreviewSkin = false;
            }
        }
    }

    public SkinData Get_SkinData(int ID)
    {
        for (int count = 0; count < SkinData_Manager.MySkinList.skins.Count; count++)
        {
            if (SkinData_Manager.MySkinList.skins[count].id == ID){
                return SkinData_Manager.MySkinList.skins[count];
            }
        }
        return null;
    }

}
