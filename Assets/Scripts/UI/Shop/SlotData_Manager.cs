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

    // 현재 Skin_id의 숭숭이 스킨 불러오기
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

        if(Slot_SkinData.is_locked){ // 잠겨있으면 금액 가져오기
            state = Slot_SkinData.price.ToString();
        }
        else if(!Slot_SkinData.is_locked){ //안잠겨있으고
            if(Slot_SkinData.is_current_PlayerSkin){ // 현재 플래이어 스킨이라면
                state = "사용중";
                Usage_Status.SetActive(true);
                // 여기서 슬롯 파란박스 추가하기
            }
            else{
                state = "장착"; // 현재 플래이어 스킨이 아니라면
                Usage_Status.SetActive(false);
                // 여기서 슬롯 파란 박스 제거
            }
        }
        Debug.Log(state);
        PurchaseBtn_Txt.GetComponent<TextMeshProUGUI>().text = state;

    }

    public void OnClick_purchase_Btn()
    {

        SkinData_Manager.Player_SkinData_ID = Skin_id; 
        GameManagerEx.Instance.player.MonkeySkinId = Skin_id; // 장착중인 player의 skinid 변경

        SkinData_Manager.clicked_slot = this.gameObject;
        
        if (Slot_SkinData.is_locked){
            Managers.Sound.Play("ButtonConfirm");
            // 스킨 구매 팝업 띄우기
            Debug.Log("구매하시겠습니까");
            Purchase_popup.SetActive(true);
            // popup 창에서 yes or no 판단
        }
        else if(!Slot_SkinData.is_locked)
        {
            if(Slot_SkinData.is_current_PlayerSkin)
            {
                Debug.Log("이미 사용중인 스킨입니다.");
                Managers.Sound.Play("ButtonConfirm");
            }
            else if(!Slot_SkinData.is_current_PlayerSkin)
            {
                Managers.Sound.Play("Equit");
                Debug.Log("스킨을 변경합니다. 스킨 변경이 완료되었습니다.");
                Reset_is_current_PlayerSkin();
                Slot_SkinData.is_current_PlayerSkin = true;
                SkinData_Manager.last_slot = SkinData_Manager.current_slot;
                SkinData_Manager.current_slot = this.gameObject;
                if (SkinData_Manager.last_slot != SkinData_Manager.current_slot)
                {
                    Change_LastSlot();// 이전 slot 파란 박스 제거, 장착으로 텍스트 변경
                }

                skinData_manager.MonkeyPrefabSKin_Change(BodySprite, TailSprite, Slot_SkinData);
                OnClick_Preview_Btn();
                PurchaseBtn_Load();
            }
        }
    }

    public void OnClick_Purchase_Yes_Btn()
    { 
        Debug.Log("구매시작합니다아아");
        Debug.Log(Slot_SkinData.price + " : 스킨 가격");

        GameManagerEx.Instance.player.Money = 10000; // 활성화 시키면 재화 10000으로 증가
        int bananacount = GameManagerEx.Instance.player.Money;
        Debug.Log(bananacount + " : 전체 바나나");
        if (bananacount >= Slot_SkinData.price){
            Debug.Log("스킨을 구매합니다.");
            Slot_SkinData.is_locked = false;
            GameManagerEx.Instance.player.Money -=  Slot_SkinData.price;
            GameManagerEx.Instance.player.AddSkinId(Skin_id); // collected_skinid에 구매한 id 추가
            PurchaseBtn_Load(); // 버튼 상태를 장착으로 바꾸기
            PreviewBtn_Img_Load();
        }
        else if (bananacount < Slot_SkinData.price){
             Debug.Log("재화가 부족합니다.");
        }
    }

    public void OnClick_Purchase_No_Btn()
    {
        Debug.Log("스킨을  구입하지 않습니다.");
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

    public void Change_LastSlot() // 이전 slot 파란 박스 제거, 장착으로 텍스트 변경
    {
        SlotData_Manager Last_slotData_manager = SkinData_Manager.last_slot.GetComponent<SlotData_Manager>();
        Last_slotData_manager.Slot_SkinData.is_current_PlayerSkin = false;
        Last_slotData_manager.Usage_Status.SetActive(false);
        Last_slotData_manager.PurchaseBtn_Txt.GetComponent<TextMeshProUGUI>().text = "장착";
    }

    public SlotData_Manager GetCurrent_SkinData()
    {
        GameObject current_slot_object = SkinData_Manager.current_slot;
        SlotData_Manager current_slotData_manager = current_slot_object.GetComponent<SlotData_Manager>();
        return current_slotData_manager;
    }

}
