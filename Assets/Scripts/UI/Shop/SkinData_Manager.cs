using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkinData_Manager : MonoBehaviour
{
    public GameObject Skin_content;

    public static int Player_SkinData_ID;
    public static GameObject last_slot = null;
    public static GameObject current_slot;
    public static GameObject clicked_slot;
    
    private string Monkey_preview_path = "Assets/Resources/Shop/Monkey_preview.prefab";
    private string Monkey_prefab_path = "Assets/Resources/Shop/Monkey_prefab.prefab";

    private static GameObject Monkey_preview;
    private static GameObject Monkey_preview_body;
    private static GameObject Monkey_preview_tail;
    private static GameObject Monkey_previewName_TXT;

    private static GameObject Monkey_prefab;
    private static GameObject Monkey_prefab_body;
    private static GameObject Monkey_prefab_tail;



    void Start()
    {
        DataLoad();
        Set_Player_SkinData_ID();
        current_slot = Get_current_slot();
        DontDestroyOnLoad(gameObject);
        //Set_Monkey();
    }

    private void DataLoad()
    {   
        Monkey_preview = GameObject.Find("Monkey_preview");
        Monkey_preview_body = Monkey_preview.transform.Find("Monkey_Body").gameObject;
        Monkey_preview_tail = Monkey_preview.transform.Find("Monkey_Tail").gameObject;
        Monkey_previewName_TXT = Monkey_preview.transform.Find("MonkeyName").gameObject.transform.Find("MonkeyName_TXT").gameObject;

        Monkey_prefab = GameObject.Find("Monkey_prefab");
        Monkey_prefab_body = Monkey_prefab.transform.Find("Monkey_Body").gameObject;
        Monkey_prefab_tail = Monkey_prefab.transform.Find("Monkey_Tail").gameObject;

        last_slot = GameObject.Find("Usage_Status");
    }

    public void MonkeyPreviewSKin_Change(Sprite BodySprite, Sprite TailSprite, SkinData currentSkinData) // 참조자로 받는건가?
    {
        Monkey_preview_body.GetComponent<Image>().sprite = BodySprite;
        Monkey_preview_tail.GetComponent<Image>().sprite = TailSprite;
        Monkey_previewName_TXT.GetComponent<TextMeshProUGUI>().text = currentSkinData.name;

        //PrefabUtility.SaveAsPrefabAsset(Monkey_preview, Monkey_preview_path);
        currentSkinData.is_current_PreviewSkin = true;
    }

    public void MonkeyPrefabSKin_Change(Sprite BodySprite, Sprite TailSprite, SkinData currentSkinData) 
    {
        Monkey_prefab_body.GetComponent<SpriteRenderer>().sprite = BodySprite;
        Monkey_prefab_tail.GetComponent<SpriteRenderer>().sprite = TailSprite;

        //PrefabUtility.SaveAsPrefabAsset(Monkey_prefab, Monkey_prefab_path);
        currentSkinData.is_current_PlayerSkin = true;
    }

    public void Set_Player_SkinData_ID()
    {
        Debug.Log(SkinData_LoadSave.MySkinList.skins.Count + "안돼왜애ㅐ");
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].is_current_PlayerSkin == true){
                Player_SkinData_ID = SkinData_LoadSave.MySkinList.skins[count].id;
            }
        }
    }

    private GameObject Get_current_slot(){
        int numOfChild = Skin_content.transform.childCount;
        GameObject child_slot = null;

        for (int i = 0; i < numOfChild; i++){
            child_slot = Skin_content.transform.GetChild(i).gameObject;
            SlotData_Manager slotData_manager = child_slot.GetComponent<SlotData_Manager>();
            if(slotData_manager.Skin_id == Player_SkinData_ID){
                return child_slot;
            }
        }
        return null;
    } 

    private void Set_Monkey(){
        SlotData_Manager currentslotData_manager = current_slot.GetComponent<SlotData_Manager>();
        Sprite monkey_BodySprite = currentslotData_manager.BodySprite;
        Sprite monkey_TailSprite = currentslotData_manager.TailSprite;
        SkinData monkey_currentSkinData = currentslotData_manager.Slot_SkinData;
        MonkeyPreviewSKin_Change(monkey_BodySprite, monkey_TailSprite, monkey_currentSkinData);
        MonkeyPrefabSKin_Change(monkey_BodySprite, monkey_TailSprite, monkey_currentSkinData);
    }
}
