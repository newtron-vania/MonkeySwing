using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class medalCutlines
{
    public List<MedalCutline> cutline;
}

[Serializable]
public class MedalCutline
{
    public int mapid;
    public List<int> cutline;
}

[Serializable]
public class medals
{
    public string bronze;
    public string silver;
    public string gold;
    public string platinum;
    public string diamond;
}


public class DataManager
{
    #region Scriptable
    [SerializeField]
    private List<SkinDataSO> skindataList;
    public List<SkinDataSO> SkindataList { get { return skindataList; } }
    private Dictionary<int, SkinDataSO> skinDict = new Dictionary<int, SkinDataSO>();

    private void CreateSkinDict()
    {
        skindataList = new List<SkinDataSO>(Resources.LoadAll<SkinDataSO>("Prefabs/SkinSO"));
        skindataList.Sort((data1, data2) => data1.SkinId.CompareTo(data2.SkinId));

        foreach (SkinDataSO skindata in skindataList)
        {
            skinDict.Add(skindata.SkinId, skindata);
        }
    }

    public SkinDataSO GetSkin(int skinid)
    {
        SkinDataSO skinData = null;
        skinDict.TryGetValue(skinid, out skinData);
        return skinData;
    }




    private Dictionary<string, ItemDataSO> itemDict = new Dictionary<string, ItemDataSO>();

    private void CreateItemDict()
    {
        List<ItemDataSO> itemDataList = new List<ItemDataSO>(Resources.LoadAll<ItemDataSO>("Prefabs/ItemSO"));
        foreach(ItemDataSO item in itemDataList)
        {
            itemDict.Add(item.name, item);
        }
    }

    public ItemDataSO GetItem(string itemName)
    {
        ItemDataSO item = null;
        itemDict.TryGetValue(itemName, out item);
        return item;
    }

    #endregion


    #region medalCut
    public Dictionary<int, MedalCutline> medalCutDict = new Dictionary<int, MedalCutline>();
    public void LoadMedalCutlines()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Json/medalDatas");

        medalCutlines medalCutlines = JsonUtility.FromJson<medalCutlines>(textAsset.text);

        foreach(MedalCutline cutline in medalCutlines.cutline)
        {
            medalCutDict[cutline.mapid] = cutline;
        }
    }
    #endregion

    #region medals
    public Dictionary<string, string> medalDict = new Dictionary<string, string>();

    public void LoadMedalPath()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Json/Medals");

        medals medals = JsonUtility.FromJson<medals>(textAsset.text);

        medalDict["bronze"] = medals.bronze;
        medalDict["silver"] = medals.silver;
        medalDict["gold"] = medals.gold;
        medalDict["platinum"] = medals.platinum;
        medalDict["diamond"] = medals.diamond;
    }


    public string GetMedalName(int num)
    {
        switch(num)
        {
            case 0:
                return "bronze";
            case 1:
                return "silver";
            case 2:
                return "gold";
            case 3:
                return "platinum";
            case 4:
                return "diamond";
        }
        return "None";
    }

    public string GetMedal(int score, int mapid)
    {
        List<int> cutline = medalCutDict[mapid].cutline;
        string medal = "bronze";
        for (int i = 0; i <= cutline.Count; i++)
        {
            if (i == cutline.Count)
            {
                medal = Managers.Data.GetMedalName(i);
                break;
            }
            if (score < cutline[i])
            {
                medal = Managers.Data.GetMedalName(i);
                break;
            }
        }

        return medal;
    }

    public string GetMedalSpritePath(int score, int mapid)
    {
        return medalDict[GetMedal(score, mapid)];
    }
    #endregion
    public void Init()
    {
        CreateSkinDict();
        CreateItemDict();
        LoadMedalCutlines();
        LoadMedalPath();
    }
}