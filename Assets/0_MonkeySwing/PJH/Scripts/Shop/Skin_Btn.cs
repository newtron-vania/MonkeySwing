using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin_Btn : MonoBehaviour
{
    public GameObject MySkinBtn, TotalSkinBtn;
    public GameObject Skin_content;

    private GameObject My_Monkey_mini, Total_Monkey_mini;
    private Button MySkin_btn_comp, TotalSkin_btn_comp;

    void Start(){
        My_Monkey_mini = MySkinBtn.transform.Find("my_Monkey_mini").gameObject;
        Total_Monkey_mini = TotalSkinBtn.transform.Find("total_Monkey_mini").gameObject;

        MySkin_btn_comp = MySkinBtn.GetComponent<Button>();
        TotalSkin_btn_comp = TotalSkinBtn.GetComponent<Button>();

    }

    public void Click_TotalSkin(){
        int numOfChild = Skin_content.transform.childCount;
        GameObject child_slot = null;

        Btn_color_Change("Total");

        for (int i = 0; i < numOfChild; i++){
            child_slot = Skin_content.transform.GetChild(i).gameObject;
            SlotData_Manager slotData_manager = child_slot.GetComponent<SlotData_Manager>();
            if(slotData_manager.Slot_SkinData.is_locked == true){
                child_slot.SetActive(true);
            }
        }
    }

    public void Click_MySkin(){
        int numOfChild = Skin_content.transform.childCount;
        GameObject child_slot = null;

        Btn_color_Change("My");

        for (int i = 0; i < numOfChild; i++){
            child_slot = Skin_content.transform.GetChild(i).gameObject;
            SlotData_Manager slotData_manager = child_slot.GetComponent<SlotData_Manager>();
            if(slotData_manager.Slot_SkinData.is_locked == true){
                child_slot.SetActive(false);
            }
        }
    }

    public void Btn_color_Change(string BTN_name){
        GameObject Able_monkey_mini = null;
        GameObject Disable_monkey_mini = null;
        Button Able_btn_comp = null;
        Button Disable_btn_comp = null;

        if(BTN_name == "Total"){
            Able_monkey_mini = Total_Monkey_mini;
            Disable_monkey_mini= My_Monkey_mini;
            
            Able_btn_comp = TotalSkin_btn_comp;
            Disable_btn_comp = MySkin_btn_comp;
        }
        else if(BTN_name == "My"){
            Able_monkey_mini = My_Monkey_mini;
            Disable_monkey_mini= Total_Monkey_mini;
            
            Able_btn_comp = MySkin_btn_comp;
            Disable_btn_comp = TotalSkin_btn_comp;  
        }
        else{
            Debug.Log("No SKIN Name");
        }

        // 클릭시 mini monkey 활성화 & 다른 몽키 비활성화
        Able_monkey_mini.SetActive(true);
        Disable_monkey_mini.SetActive(false);
        
        // 클릭시 갈색으로 색 바꾸기
        ColorBlock Able_colorBlock = Able_btn_comp.colors;
        Able_colorBlock.normalColor = new Color(142/ 255f, 100/ 255f, 89/ 255f, 255 / 255f);
        Able_colorBlock.highlightedColor = new Color(142/ 255f, 100/ 255f, 89/ 255f, 255 / 255f);
        Able_colorBlock.pressedColor = new Color(142/ 255f, 100/ 255f, 89/ 255f, 255 / 255f);
        Able_colorBlock.selectedColor = new Color(142/ 255f, 100/ 255f, 89/ 255f, 255 / 255f);
        Able_colorBlock.disabledColor = new Color(142/ 255f, 100/ 255f, 89/ 255f, 255 / 255f);
        Able_btn_comp.colors = Able_colorBlock;

        // 다른 버튼 흰색으로 바꾸기
        ColorBlock Disable_colorBlock = Disable_btn_comp.colors;
        Disable_colorBlock.normalColor = new Color(255f, 255f, 255f, 255f);
        Disable_colorBlock.highlightedColor = new Color(255f, 255f, 255f, 255f);
        Disable_colorBlock.pressedColor = new Color(255f, 255f, 255f, 255f);
        Disable_colorBlock.selectedColor = new Color(255f, 255f, 255f, 255f);
        Disable_colorBlock.disabledColor = new Color(255f, 255f, 255f, 255f);
        Disable_btn_comp.colors = Disable_colorBlock;
    }
}
