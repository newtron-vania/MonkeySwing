using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public enum SlotState
{
    Init,
    isPurchased,
    isUsing,
}

public enum SkinState
{
    MySkin,
    TotalSkin,
}

public class ShopSkinManager : Singleton<ShopSkinManager>
{
    public Slot currentSlot;
    public List<Slot> slots;

    [SerializeField] private Image previewMonkeyBody;
    [SerializeField] private Image previewMonkeyTail;
    [SerializeField] private TextMeshProUGUI previewMonkeyName;
    [SerializeField] private TextMeshProUGUI previewMonkeyExplanation;

    [SerializeField] private GameObject purchasePopUp;
    [SerializeField] private Sprite isUsingImg;
    [SerializeField] private Sprite isPurchasedImg;

    [SerializeField] private GameObject PurchaseComplete;

    [SerializeField] private SkinState _currentSkinState;
    [SerializeField] private MyButtonImageController _mySkinButton;

    private void Awake()
    {
        GameManagerEx.Instance.player.Money += 10000; // TEST용 이거 지우기
        _currentSkinState = SkinState.TotalSkin;
    }

    private void Start()
    {
        LoadData();
        foreach (Slot slot in slots)
        {
            // player의 skinid와 같은 slot을 현재 slot으로 지정
            if (slot.MonkeyId == GameManagerEx.Instance.player.MonkeySkinId)
            {
                currentSlot = slot;
            }
        }
        currentSlot.SlotStateButtonClick();
    }

    private void LoadData()
    {
        List<int> skinlist = GameManagerEx.Instance.player.GetSkinIds();
        foreach (Slot slot in slots)
        {
            if (skinlist.Contains(slot.MonkeyId))
            {
                slot.slotState = SlotState.isPurchased;
                setSlotisPurchased(slot);
            }
        }
    }

    // currentSlot으로 preview 정보 바꾸기
    public void PreviewChange()
    {
        previewMonkeyBody.sprite = currentSlot.MonkeyPreviewBody;
        previewMonkeyTail.sprite = currentSlot.MonkeyPreviewTail;
        previewMonkeyName.text = currentSlot.MonkeyName;
        previewMonkeyExplanation.text = currentSlot.MonkeyExplanation;
    }

    public void SlotStateChange()
    {
        switch (currentSlot.slotState)
        {
            case (SlotState.Init): // 초기 state이면 => 구매 팝업 띄우기
                PurchaseSkin();
                break;
            case (SlotState.isPurchased): // 이미 구매된 스킨(장착)이면 => 사용중(사용중)으로 변경
                isUsingStateClear();
                currentSlot.slotState = SlotState.isUsing;
                currentSlot.selectSlotImg.gameObject.SetActive(true);
                currentSlot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "사용중";
                currentSlot.slotStateButton.GetComponent<Image>().sprite = isUsingImg;
                break;
            case (SlotState.isUsing):  // 현재 사용중(사용중)이면 => 이미 장착중인 skin이라고 debug 띄우기
                ActiveDebug("이미 사용중인 스킨입니다.");
                break;
        }
    }

    // slot의 state가 isUsingState이면 isPurchased로 변경  
    public void isUsingStateClear()
    {
        foreach (Slot slot in slots)
        {
            if (slot.slotState == SlotState.isUsing)
            {
                GameManagerEx.Instance.player.MonkeySkinId = slot.MonkeyId;

                slot.slotState = SlotState.isPurchased;
                slot.selectSlotImg.gameObject.SetActive(false);
                slot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
                slot.slotStateButton.GetComponent<Image>().sprite = isPurchasedImg;
            }
        }
    }

    private void setSlotisPurchased(Slot slot)
    {
        slot.slotState = SlotState.isPurchased;
        slot.lockImg.gameObject.SetActive(false);
        slot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
    }

    // slot의 skin 구매하기
    public void PurchaseSkin()
    {
        if (GameManagerEx.Instance.player.Money >= RarelityToPrice())
        { 
            // 팝업창 띄우기
            purchasePopUp.SetActive(true);
        }
        else 
        {
            ActiveDebug("재화가 부족해 구매할 수 없습니다.");
        }
    }

    public void PurchaseSkinYesClick()
    {
        // currentSlot의 skin구매

        GameManagerEx.Instance.player.Money -= RarelityToPrice();
        GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.Money;

        GameManagerEx.Instance.player.MonkeySkinId = currentSlot.MonkeyId;
        GameManagerEx.Instance.player.AddSkinId(currentSlot.MonkeyId);

        ActiveDebug("스킨 구매가 완료되었습니다.");
        currentSlot.slotState = SlotState.isPurchased;
        currentSlot.lockImg.gameObject.SetActive(false);
        currentSlot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착";

        purchasePopUp.SetActive(false);
    }

    public void PurchaseSkinNoClick()
    {
        purchasePopUp.SetActive(false);
    }

    public void TotalSkinViewClick()
    {
        _currentSkinState = SkinState.TotalSkin;
        ChangeImage();
        foreach (Slot slot in slots)
        {
            if (!slot.gameObject.activeInHierarchy)
            {
                slot.gameObject.SetActive(true);
            }
        }
    }

    public void MySkinViewClick()
    {
        _currentSkinState = SkinState.MySkin;
        ChangeImage();
        foreach (Slot slot in slots)
        {
            if (slot.slotState == SlotState.Init)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickMySkinButton()
    {
        if (_currentSkinState == SkinState.TotalSkin)
        {
            MySkinViewClick();
        }
        else if (_currentSkinState == SkinState.MySkin)
        {
            TotalSkinViewClick();
        }
    }

    private void ChangeImage()
    {
        if (_currentSkinState == SkinState.TotalSkin)
        {
            _mySkinButton.ChangeImage(false);
        }
        else if (_currentSkinState == SkinState.MySkin)
        {
            _mySkinButton.ChangeImage(true);
        }
    }

    // 등급에 따라 가격으로 변환
    private int RarelityToPrice()
    {
        switch (currentSlot.MonkeyRarelity) 
        {
            case (Define.Rarelity.Normal): return 1200;
            case (Define.Rarelity.Rare): return 1700;
            case (Define.Rarelity.Epic): return 3000;
            default: return 0;
        }
    }

    private void ActiveDebug(String debugDetail)
    {
        PurchaseComplete.GetComponentInChildren<TextMeshProUGUI>().text = debugDetail;
        PurchaseComplete.gameObject.SetActive(true);

        StartCoroutine(DeactiveDebug());
    }

    private IEnumerator DeactiveDebug()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        PurchaseComplete.gameObject.SetActive(false);
    }
}
