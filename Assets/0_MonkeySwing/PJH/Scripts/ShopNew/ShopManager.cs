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
    TotalSkin,
    MySkin,
}


public class ShopManager : Singleton<ShopManager>
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


    [SerializeField] private GameObject TotalSkinButton;
    [SerializeField] private GameObject MySkinButton;

    private GameObject currrentSkinButtonMiniMonkey;
    private GameObject otherSkinButtonMiniMonkey;

    [SerializeField] private TextMeshProUGUI DebugPanel;

    private void Awake()
    {

        // ó�� Ű�� totalSkinâ�� ���� �ߵ��� ����
        currrentSkinButtonMiniMonkey = TotalSkinButton;
        otherSkinButtonMiniMonkey = MySkinButton;
        SetSkinState();

        LoadData();
        foreach (Slot slot in slots)
        {
            // player�� skinid�� ���� slot�� ���� slot���� ����
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
        //for (int i = 0; i < MySkinList.skins.Count; i++)

        {
            if (skinlist.Contains(slot.MonkeyId))
            {
                slot.slotState = SlotState.isPurchased;
                setSlotisPurchased(slot);
            }
        }
    }

    // currentSlot���� preview ���� �ٲٱ�
    public void PreviewChange()
    {
        previewMonkeyBody.sprite = currentSlot.MonkeyPreviewBody;
        previewMonkeyTail.sprite = currentSlot.MonkeyPreviewTail;
        previewMonkeyName.text = currentSlot.MonkeyName;
        previewMonkeyExplanation.text = currentSlot.MonkeyExplanation;
    }
    public void SlotStateChange(int MonkeyId)
    {
        switch (currentSlot.slotState)
        {
            case (SlotState.Init): // �ʱ� state�̸� => ���� �˾� ����
                PurchaseSkin();
                break;
            case (SlotState.isPurchased): // �̹� ���ŵ� ��Ų(����)�̸� => �����(�����)���� ����
                isUsingStateClear();
                currentSlot.slotState = SlotState.isUsing;
                currentSlot.selectSlotImg.gameObject.SetActive(true);
                currentSlot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "�����";
                currentSlot.slotStateButton.GetComponent<Image>().sprite = isUsingImg;
                GameManagerEx.Instance.player.MonkeySkinId = MonkeyId;
                break;
            case (SlotState.isUsing):  // ���� �����(�����)�̸� => �̹� �������� skin�̶�� debug ����
                ActiveDebug("�̹� ������� ��Ų�Դϴ�.");
                break;
        }
    }

    // slot�� state�� isUsingState�̸� isPurchased�� ����  
    public void isUsingStateClear()
    {
        foreach (Slot slot in slots)
        {
            if (slot.slotState == SlotState.isUsing)
            {
                GameManagerEx.Instance.player.MonkeySkinId = slot.MonkeyId;

                slot.slotState = SlotState.isPurchased;
                slot.selectSlotImg.gameObject.SetActive(false);
                slot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
                slot.slotStateButton.GetComponent<Image>().sprite = isPurchasedImg;
            }
        }
    }
    private void setSlotisPurchased(Slot slot)
    {
        slot.slotState = SlotState.isPurchased;
        slot.lockImg.gameObject.SetActive(false);
        slot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";
    }


    // slot�� skin �����ϱ�
    public void PurchaseSkin()
    {
        if (GameManagerEx.Instance.player.Money >= RarelityToPrice())
        { 
            // �˾�â ����
            purchasePopUp.SetActive(true);
        }
        else 
        {
            ActiveDebug("��ȭ�� ������ ������ �� �����ϴ�.");
        }
    }

    public void PurchaseSkinYesClick()
    {
        // currentSlot�� skin����

        GameManagerEx.Instance.player.Money -= RarelityToPrice();
        GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.Money;

        GameManagerEx.Instance.player.MonkeySkinId = currentSlot.MonkeyId;
        GameManagerEx.Instance.player.AddSkinId(currentSlot.MonkeyId);

        ActiveDebug("��Ų ���Ű� �Ϸ�Ǿ����ϴ�.");
        currentSlot.slotState = SlotState.isPurchased;
        currentSlot.lockImg.gameObject.SetActive(false);
        currentSlot.slotStateButton.GetComponentInChildren<TextMeshProUGUI>().text = "����";

        purchasePopUp.SetActive(false);
    }

    public void PurchaseSkinNoClick()
    {
        purchasePopUp.SetActive(false);
    }

    public void TotalSkinViewClick()
    {
        currrentSkinButtonMiniMonkey = TotalSkinButton;
        otherSkinButtonMiniMonkey = MySkinButton;
        SetSkinState();

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
        otherSkinButtonMiniMonkey = TotalSkinButton;
        currrentSkinButtonMiniMonkey = MySkinButton;
        SetSkinState();

        foreach (Slot slot in slots)
        {
            if (slot.slotState == SlotState.Init)
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    public void SetSkinState()
    {
        currrentSkinButtonMiniMonkey.transform.parent.GetComponent<Image>().color = Color.gray;
        currrentSkinButtonMiniMonkey.SetActive(true);

        otherSkinButtonMiniMonkey.transform.parent.GetComponent<Image>().color = Color.white;
        otherSkinButtonMiniMonkey.SetActive(false);
    }



    // ��޿� ���� �������� ��ȯ
    private int RarelityToPrice()
    {
        switch (currentSlot.MonkeyRarelity) 
        {
            case (Define.Rarelity.Normal): return 1000;
            case (Define.Rarelity.Rare): return 3000;
            case (Define.Rarelity.Epic): return 5000;
            default: return 0;
        }
    }

    private void ActiveDebug(String debugDetail)
    {
        DebugPanel.text = debugDetail;
        DebugPanel.gameObject.SetActive(true);

        StartCoroutine(DeactiveDebug());
    }

    private IEnumerator DeactiveDebug()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        DebugPanel.gameObject.SetActive(false);
    }
}
