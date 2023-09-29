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
            itemDict[item.itemName.ToString()] = item;
        }
    }

    #endregion

    #region Skill
    private Dictionary<int, BaseSkill> _skillDict = new Dictionary<int, BaseSkill>();
    Dictionary<int, BaseSkill> _skillCopy;
    public void SetSkillDict()
    {
        _skillDict.Add(new BasicSkill().GetSkillId(), new BasicSkill());
        _skillDict.Add(new PinkSkill().GetSkillId(), new PinkSkill());
        _skillDict.Add(new YellowSkill().GetSkillId(), new YellowSkill());
        _skillDict.Add(new BlueSkill().GetSkillId(), new BlueSkill());
        _skillDict.Add(new MilkSkill().GetSkillId(), new MilkSkill());
        _skillDict.Add(new HeartSkill().GetSkillId(), new HeartSkill());
        _skillDict.Add(new CloudSkill().GetSkillId(), new CloudSkill());
    }

    public void ClearSkillDict()
    {
        Dictionary<int, BaseSkill> _skillCopy = new Dictionary<int, BaseSkill>(_skillDict);
    }

    public BaseSkill GetSkill(int skillId)
    {
        ClearSkillDict();
        return _skillCopy[skillId];
    }

    #endregion

    #region MedalCut
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
        string medal = "none";
        for (int i = 0; i < cutline.Count; i++)
        {
            if (score < cutline[i])
            {
                break;
            }
            medal = Managers.Data.GetMedalName(i);
        }

        return medal;
    }

    public string GetMedalSpritePath(int score, int mapid)
    {
        string medalName = GetMedal(score, mapid);
        if(medalName == "none")
        {
            return "None_Medal";
        }
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
        ClearSkillDict();
    }
}