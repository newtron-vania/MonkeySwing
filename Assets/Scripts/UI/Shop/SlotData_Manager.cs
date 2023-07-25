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


public class SlotData_Manager : MonoBehaviour
    {
    public int Skin_id;
    private static int currentSkin_ID;
    private PlayerData player = new PlayerData();

    private SkinData_Manager skinData_manager = new SkinData_Manager();

    public Sprite BodySprite;
    public Sprite TailSprite;
    
    public SkinData Slot_SkinData;
    [SerializeField]
    GameObject PreviewBtn;
    GameObject Purchase_popup;
    [SerializeField]
    GameObject PurchaseBtn;
    [SerializeField]
    GameObject PurchaseBtn_Txt;
    [SerializeField]
    GameObject Usage_Status;
   

    public void init(SkinData_Manager skindataManager)
    {
        Slot_SkinData = Get_SkinData(Skin_id);
        SkinImg_Load();
        skinData_manager = skindataManager;
    }
    void Start()
    {
        Purchase_popup = GameObject.Find("Shop").transform.Find("Purchase_popup").gameObject;
;
        
        PreviewBtn_Img_Load();
        PurchaseBtn_Load();        
    }

    // ���� Skin_id�� ������ ��Ų �ҷ�����
    public void SkinImg_Load()
    {
        string body_path = Slot_SkinData.monkey_skin_path[0];
        string tail_path = Slot_SkinData.monkey_skin_path[1];
        BodySprite = Resources.Load<Sprite>(body_path);
        TailSprite = Resources.Load<Sprite>(tail_path);
    }

    public void PreviewBtn_Img_Load()
    {
        string PreviewBtn_path = "";
        if(Slot_SkinData.is_locked){
            PreviewBtn_path = Slot_SkinData.slot_lock_img_path;
        }
        else if(!Slot_SkinData.is_locked){
            PreviewBtn_path = Slot_SkinData.slot_unlock_img_path;
        }

        Sprite PreviewBtn_Img = Resources.Load<Sprite>(PreviewBtn_path);
        PreviewBtn.GetComponent<Image>().sprite = PreviewBtn_Img;
    }

    public void PurchaseBtn_Load()
    {
        string state = "";
        Slot_SkinData_Load();
        Debug.Log(Slot_SkinData.is_locked);

        if(Slot_SkinData.is_locked){ // ��������� �ݾ� ��������
            state = Slot_SkinData.price.ToString();
        }
        else if(!Slot_SkinData.is_locked){ //�����������
            if(Slot_SkinData.is_current_PlayerSkin){ // ���� �÷��̾� ��Ų�̶��
                state = "�����";
                Usage_Status.SetActive(true);
                // ���⼭ ���� �Ķ��ڽ� �߰��ϱ�
            }
            else{
                state = "����"; // ���� �÷��̾� ��Ų�� �ƴ϶��
                Usage_Status.SetActive(false);
                // ���⼭ ���� �Ķ� �ڽ� ����
            }
        }
        Debug.Log(state);
        PurchaseBtn_Txt.GetComponent<TextMeshProUGUI>().text = state;

    }

    public void OnClick_purchase_Btn()
    {

        SkinData_Manager.Player_SkinData_ID = Skin_id; 
        GameManagerEx.Instance.player.MonkeySkinId = Skin_id; // �������� player�� skinid ����

        SkinData_Manager.clicked_slot = this.gameObject;
        
        if (Slot_SkinData.is_locked){
            Managers.Sound.Play("ButtonConfirm");
            // ��Ų ���� �˾� ����
            Debug.Log("�����Ͻðڽ��ϱ�");
            Purchase_popup.SetActive(true);
            // popup â���� yes or no �Ǵ�
        }
        else if(!Slot_SkinData.is_locked)
        {
            if(Slot_SkinData.is_current_PlayerSkin)
            {
                Debug.Log("�̹� ������� ��Ų�Դϴ�.");
                Managers.Sound.Play("ButtonConfirm");
            }
            else if(!Slot_SkinData.is_current_PlayerSkin)
            {
                Managers.Sound.Play("Equit");
                Debug.Log("��Ų�� �����մϴ�. ��Ų ������ �Ϸ�Ǿ����ϴ�.");
                Reset_is_current_PlayerSkin();
                Slot_SkinData.is_current_PlayerSkin = true;
                SkinData_Manager.last_slot = SkinData_Manager.current_slot;
                SkinData_Manager.current_slot = this.gameObject;
                if (SkinData_Manager.last_slot != SkinData_Manager.current_slot)
                {
                    Change_LastSlot();// ���� slot �Ķ� �ڽ� ����, �������� �ؽ�Ʈ ����
                }

                skinData_manager.MonkeyPrefabSKin_Change(BodySprite, TailSprite, Slot_SkinData);
                OnClick_Preview_Btn();
                PurchaseBtn_Load();
            }
        }
    }

    public void OnClick_Purchase_Yes_Btn()
    { 
        Debug.Log("���Ž����մϴپƾ�");
        Debug.Log(Slot_SkinData.price + " : ��Ų ����");

        GameManagerEx.Instance.player.Money = 10000; // Ȱ��ȭ ��Ű�� ��ȭ 10000���� ����
        int bananacount = GameManagerEx.Instance.player.Money;
        Debug.Log(bananacount + " : ��ü �ٳ���");
        if (bananacount >= Slot_SkinData.price){
            Debug.Log("��Ų�� �����մϴ�.");
            Slot_SkinData.is_locked = false;
            GameManagerEx.Instance.player.Money -=  Slot_SkinData.price;
            GameManagerEx.Instance.player.AddSkinId(Skin_id); // collected_skinid�� ������ id �߰�
            PurchaseBtn_Load(); // ��ư ���¸� �������� �ٲٱ�
            PreviewBtn_Img_Load();
        }
        else if (bananacount < Slot_SkinData.price){
             Debug.Log("��ȭ�� �����մϴ�.");
        }
    }

    public void OnClick_Purchase_No_Btn()
    {
        Debug.Log("��Ų��  �������� �ʽ��ϴ�.");
    }

    public void OnClick_Preview_Btn()
    {
        Managers.Sound.Play("ButtonConfirm");
        // PreviewBtn_Img_Load();
        if (!Slot_SkinData.is_current_PreviewSkin)
        {
            //MonkeyPreviewSKin_Change();
            skinData_manager.MonkeyPreviewSKin_Change(BodySprite, TailSprite, Slot_SkinData);
            Reset_is_current_PreviewSkin();
            Slot_SkinData.is_current_PreviewSkin = true;
        }
    }


    public void Reset_is_current_PlayerSkin()
    {
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].is_current_PlayerSkin == true){
                SkinData_LoadSave.MySkinList.skins[count].is_current_PlayerSkin = false;
            }
        }
    }

    public void Reset_is_current_PreviewSkin()
    {
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].is_current_PreviewSkin == true){
                SkinData_LoadSave.MySkinList.skins[count].is_current_PreviewSkin = false;
            }
        }
    }

    public int GetCurrentPreviewID()
    {
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].is_current_PreviewSkin == true){
                return SkinData_LoadSave.MySkinList.skins[count].id;
            }
        }
        return 0;
    }


    public SkinData Get_SkinData(int ID)
    {
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].id == ID){
                return SkinData_LoadSave.MySkinList.skins[count];
            }
        }
        return null;
    }
    
    public void Slot_SkinData_Load()
    {
        if (Slot_SkinData == null){
            Slot_SkinData = Get_SkinData(SkinData_Manager.Player_SkinData_ID);
        }
    }

    public void Change_LastSlot() // ���� slot �Ķ� �ڽ� ����, �������� �ؽ�Ʈ ����
    {
        SlotData_Manager Last_slotData_manager = SkinData_Manager.last_slot.GetComponent<SlotData_Manager>();
        Last_slotData_manager.Slot_SkinData.is_current_PlayerSkin = false;
        Last_slotData_manager.Usage_Status.SetActive(false);
        Last_slotData_manager.PurchaseBtn_Txt.GetComponent<TextMeshProUGUI>().text = "����";
    }

    public SlotData_Manager GetCurrent_SkinData()
    {
        GameObject current_slot_object = SkinData_Manager.current_slot;
        SlotData_Manager current_slotData_manager = current_slot_object.GetComponent<SlotData_Manager>();
        return current_slotData_manager;
    }

}
