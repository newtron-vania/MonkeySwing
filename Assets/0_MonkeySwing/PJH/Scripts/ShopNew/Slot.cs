using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int MonkeyId;
    public string MonkeyName;
    public Define.Rarelity MonkeyRarelity;
    [TextArea] public string MonkeyExplanation;

    public Sprite MonkeyPreviewTail;
    public Sprite MonkeyPreviewBody;

    public SlotState slotState = SlotState.Init;

    public Image lockImg;
    public Image selectSlotImg;
    public GameObject slotStateButton;

    public void PreviewButtonClick()
    {
        ShopManager.instance.currentSlot = this;
        ShopManager.instance.PreviewChange();
    }

    public void SlotStateButtonClick()
    {
        PreviewButtonClick();
        ShopManager.instance.SlotStateChange(MonkeyId);
    }
}
