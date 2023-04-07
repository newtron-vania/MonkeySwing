using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
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
        Debug.Log("SetSkinStart");
        foreach (KeyValuePair<int, SkinDataSO> keyValue in skinDict)
        {
            Debug.Log($"skinid : {keyValue.Key} , data : {keyValue.Value.SkinHead}");
        }
    }

    public SkinDataSO GetSkin(int skinid)
    {
        Debug.Log("GetSkinStart");
        foreach (KeyValuePair<int, SkinDataSO> keyValue in skinDict)
        {
            Debug.Log($"skinid : {keyValue.Key} , data : {keyValue.Value.SkinHead}");
        }
        SkinDataSO skinData = null;
        Debug.Log($"skin is not null :{skinDict.TryGetValue(skinid, out skinData)}");
        Debug.Log($"Get skinData id {skinData.SkinId}");
        return skinData;
    }

    public void Init()
    {
        CreateSkinDict();
    }
}

