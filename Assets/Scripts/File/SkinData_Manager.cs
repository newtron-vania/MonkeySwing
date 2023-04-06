using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkinData_Manager : MonoBehaviour
{
    public static int Player_SkinData_ID;
    public static GameObject last_slot = null;
    public static GameObject current_slot;
    
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
        Player_SkinData_ID = Set_Player_SkinData_ID();
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

        currentSkinData.is_current_PreviewSkin = true;
    }

    public void MonkeyPrefabSKin_Change(Sprite BodySprite, Sprite TailSprite, SkinData currentSkinData) 
    {
        Monkey_prefab_body.GetComponent<SpriteRenderer>().sprite = BodySprite;
        Monkey_prefab_tail.GetComponent<SpriteRenderer>().sprite = TailSprite;

        currentSkinData.is_current_PlayerSkin = true;
    }

    public int Set_Player_SkinData_ID()
    {
        Debug.Log(SkinData_LoadSave.MySkinList.skins.Count + "안돼왜애ㅐ");
        for (int count = 0; count < SkinData_LoadSave.MySkinList.skins.Count; count++)
        {
            if (SkinData_LoadSave.MySkinList.skins[count].is_current_PlayerSkin == true){
                return SkinData_LoadSave.MySkinList.skins[count].id;
            }
        }
        return 0;
    }

}
