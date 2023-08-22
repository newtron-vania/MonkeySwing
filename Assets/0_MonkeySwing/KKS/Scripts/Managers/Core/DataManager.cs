using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

[Serializable]
public class medalCutlines
{
    public List<MedalCutline> medalcutlines;
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


    public void CreateSkinDict()
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




    public Dictionary<string, ItemDataSO> itemDict = new Dictionary<string, ItemDataSO>();

    public void CreateItemDict()
    {
        List<ItemDataSO> itemDataList = new List<ItemDataSO>(Resources.LoadAll<ItemDataSO>("Prefabs/ItemSO"));
        foreach(ItemDataSO item in itemDataList)
        {
            itemDict.Add(item.name, item);
        }
    }

    #endregion

    #region Skill
    public Dictionary<int, BaseSkill> skillDict = new Dictionary<int, BaseSkill>();

    public void SetSkillDict()
    {
        skillDict.Add(new BasicSkill().GetSkillId(), new BasicSkill());
        skillDict.Add(new PinkSkill().GetSkillId(), new PinkSkill());
        skillDict.Add(new YellowSkill().GetSkillId(), new YellowSkill());
        skillDict.Add(new BlueSkill().GetSkillId(), new BlueSkill());
        skillDict.Add(new MilkSkill().GetSkillId(), new MilkSkill());
        skillDict.Add(new HeartSkill().GetSkillId(), new HeartSkill());
        skillDict.Add(new CloudSkill().GetSkillId(), new CloudSkill());
    }

    #endregion

    #region medalCut
    public Dictionary<int, MedalCutline> medalCutDict = new Dictionary<int, MedalCutline>();
    public void LoadMedalCutlines()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Json/medalDatas");

        medalCutlines medalcutlines = JsonUtility.FromJson<medalCutlines>(textAsset.text);
        Debug.Log(textAsset.text);

        foreach(MedalCutline cutline in medalcutlines.medalcutlines)
        {
            medalCutDict[cutline.mapid] = cutline;
            Debug.Log(cutline.mapid + " cutlines ");
            foreach(int num in medalCutDict[cutline.mapid].cutline)
            {
                Debug.Log(num);
            }
        }
    }
    #endregion

    #region medals
    public Dictionary<string, string> medalDict = new Dictionary<string, string>();

    public void LoadMedalPath()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("Json/Medals");

        medals medals = JsonUtility.FromJson<medals>(textAsset.text);
        Debug.Log(textAsset.text);

        medalDict["bronze"] = medals.bronze;
        medalDict["silver"] = medals.silver;
        medalDict["gold"] = medals.gold;
        medalDict["platinum"] = medals.platinum;
        medalDict["diamond"] = medals.diamond;

        Debug.Log("medals path dictionary");
        foreach(var kv in medalDict)
        {
            Debug.Log($"{kv.Key} : {kv.Value}");
        }
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
        SetSkillDict();
    }
}