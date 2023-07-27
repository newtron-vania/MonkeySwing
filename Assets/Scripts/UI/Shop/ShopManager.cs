using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Newtonsoft.Json;
using System.Reflection;
using TMPro;

[System.Serializable]
public class Skin
{
    public Skin(string _Id, string _Name, string _Price, bool _isUnlocked, bool _isUsing)
    { Id = _Id; Name = _Name; Price = _Price; isUnlocked = _isUnlocked; isUsing = _isUsing; }

    public string Id, Name, Price;
    public bool isUnlocked, isUsing;
}

public class ShopManager : MonoBehaviour
{
    [SerializeField] private TextAsset SkinDatabase;
    [SerializeField] private List<Skin> SkinList, MySkinList;
    [SerializeField] private string curType = "Skin";

    [SerializeField] private GameObject[] SkinSlot, MySlot;
    [SerializeField] private GameObject[] SkinUsingImage, MyUsingImage;

    //[SerializeField] private Image[] LockedSlotImage, UnlockedSlotImage;
    [SerializeField] private Sprite[] LockedSlotSprite, UnlockedSlotSprite;
    [SerializeField] private SkinStateManager skinStateManager;

    void Start()
    {
        Load();
    }

    public void PreviewClick()
    {


    }

    public void PurchaseClick()
    {

    }



    public void TabClick(SkinState skinstate)
    {
        skinStateManager.ChangeState(skinstate);
    }

    private void SlotShow(GameObject[] Slot, List<Skin> CurSkinList, GameObject[] UsingImage)
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            // 슬롯과 텍스트 보이기
            bool isExist = i < CurSkinList.Count;
            Slot[i].SetActive(isExist);
            Slot[i].GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = isExist ? CurSkinList[i].Price : "";

            // 아이템 이미지와 사용중인지 보이기
            if (isExist)
            {
                if (CurSkinList[i].isUnlocked)
                {
                    Slot[i].transform.GetChild(0).GetComponent<Image>().sprite = UnlockedSlotSprite[CurSkinList.FindIndex(x => x.Name == CurSkinList[i].Name)];

                }
                else if (!CurSkinList[i].isUnlocked)
                {
                    Slot[i].transform.GetChild(0).GetComponent<Image>().sprite = LockedSlotSprite[CurSkinList.FindIndex(x => x.Name == CurSkinList[i].Name)];
                }
                if (CurSkinList[i].isUsing) // 파란상자 표시
                {
                    UsingImage[i].SetActive(CurSkinList[i].isUsing);
                }
            }
        }
    }

    void Save()
    {
        string jdata = JsonConvert.SerializeObject(MySkinList);
        File.WriteAllText(Application.dataPath + "/Resources/Shop/ShopDatabase.txt", jdata);

        TabClick(skinStateManager.CurrentState);
    }

    void Load()
    {
        // 전체 아이템 리스트 불러오기
        string[] line = SkinDatabase.text.Substring(0, SkinDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            SkinList.Add(new Skin(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
            if (SkinList[i].isUnlocked)
            {
                MySkinList.Add(new Skin(row[0], row[1], row[2], row[3] == "TRUE", row[4] == "TRUE"));
            }
        }

        SlotShow(SkinSlot, SkinList, SkinUsingImage);
        SlotShow(MySlot, MySkinList, MyUsingImage);

        TabClick(skinStateManager.CurrentState);

        //string jdata = File.ReadAllText(Application.dataPath + "/Resources/Shop/ShopDatabase.txt");
        //MySkinList = JsonConvert.DeserializeObject<List<Skin>>(jdata);
    }

}
