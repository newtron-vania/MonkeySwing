using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartItem : MonoBehaviour
{
    Dictionary<string, ItemDataSO> item;
    private bool testing = true;

    public void Init()
    {
        item = Managers.Data.itemDict;
        if (testing)
        {
            GameManagerEx.Instance.player.SetItem(Define.Items.CaloryBanana, 1);
            GameManagerEx.Instance.player.SetItem(Define.Items.Boost, 1);
            GameManagerEx.Instance.player.SetItem(Define.Items.Magnet, 1);
        }
            
        SetItemEvent();
    }

    private void SetItemEvent()
    {
        foreach(Define.Items itemId in System.Enum.GetValues(typeof(Define.Items)))
        {
            if (GameManagerEx.Instance.player.UseItem(itemId))
            {
                switch (itemId)
                {
                    case Define.Items.CaloryBanana:
                        SetCaloryEvent();
                        break;
                    case Define.Items.Boost:
                        SetBoostEvent();
                        break;
                    case Define.Items.Magnet:
                        SetMagnetEvent();
                        break;
                }
            }
        }
    }

    private void SetCaloryEvent()
    {
        GameManagerEx.Instance.monkey.Weight = 
            Mathf.Min(GameManagerEx.Instance.monkey.Weight + Managers.Data.itemDict["CaloryBanana"].value, 
            GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[3]);
    }

    private void SetMagnetEvent()
    {
        GameObject go = Managers.Resource.Instantiate("Item/MagnetField", GameManagerEx.Instance.monkey.transform.position, GameManagerEx.Instance.monkey.transform);
    }

    private void SetBoostEvent()
    {
        GameObject go = Managers.Resource.Instantiate("Item/BoostEffect");
    }

}
