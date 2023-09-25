using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemManager : Singleton<ShopItemManager>
{
    public enum ItemState
    {
        MyItem,
        TotalItem,
    }

    [SerializeField] private GameObject _totalItemView;
    [SerializeField] private GameObject _myItemView;

    [SerializeField] private ItemState _currentItemState;
    [SerializeField] private MyButtonImageController _myItemButton;


    void Start()
    {
        _currentItemState = ItemState.TotalItem;
    }

    public void TotalItemViewClick()
    {
        _currentItemState = ItemState.TotalItem;
        ChangeView();
    }

    public void MySkinViewClick()
    {
        _currentItemState = ItemState.MyItem;
        ChangeView();
    }

    public void OnClickMySkinButton()
    {
        if (_currentItemState == ItemState.TotalItem)
        {
            MySkinViewClick();
        }
        else if (_currentItemState == ItemState.MyItem)
        {
            TotalItemViewClick();
        }
    }
    private void ChangeView()
    {
        if (_currentItemState == ItemState.TotalItem)
        {
            _myItemButton.ChangeImage(false);
            _totalItemView.SetActive(true);
            _myItemView.SetActive(false);

        }
        else if (_currentItemState == ItemState.MyItem)
        {
            _myItemButton.ChangeImage(true);
            _totalItemView.SetActive(false);
            _myItemView.SetActive(true);
        }
    }
}
