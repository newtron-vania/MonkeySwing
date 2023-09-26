using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    public enum ShopState
    {
        Skin,
        Item,
    }

    [SerializeField] private GameObject totalSkinView;
    [SerializeField] private GameObject totalItemView;

    [SerializeField] private GameObject TotalSkinButton;
    [SerializeField] private GameObject TotalItemButton;

    private ShopState _currentShopState;


    void Start()
    {
        _currentShopState = ShopState.Skin;
        ChangeView();
    }

    public void TotalSkinViewClick()
    {
        _currentShopState = ShopState.Skin;
        ChangeView();
        ShopSkinManager.instance.TotalSkinViewClick();
    }

    public void TotalItemViewClick()
    {
        _currentShopState = ShopState.Item;
        ChangeView();
        ShopItemManager.instance.TotalItemViewClick();
    }

    private void ChangeView()
    {
        if (_currentShopState == ShopState.Skin)
        {
            totalSkinView.SetActive(true);
            totalItemView.SetActive(false);

            TotalSkinButton.GetComponent<Image>().color = Color.gray;
            TotalSkinButton.FindChild("Monkey_mini").SetActive(true);
            TotalItemButton.GetComponent<Image>().color = Color.white;
            TotalItemButton.FindChild("Monkey_mini").SetActive(false);
        }
        else if (_currentShopState == ShopState.Item)
        {
            totalSkinView.SetActive(false);
            totalItemView.SetActive(true);

            TotalItemButton.GetComponent<Image>().color = Color.gray;
            TotalItemButton.FindChild("Monkey_mini").SetActive(true);
            TotalSkinButton.GetComponent<Image>().color = Color.white;
            TotalSkinButton.FindChild("Monkey_mini").SetActive(false);

        }
    }

}
