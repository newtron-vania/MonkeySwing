using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private List<SkinDataSO> skindataList;

    private void Start()
    {
        skindataList.Sort((data1, data2) => data1.SkinId.CompareTo(data2.SkinId));

        foreach (SkinDataSO skindata in skindataList)
        {
            Debug.Log($"id : {skindata.SkinId}, name : {skindata.SkinHead}");
        }
    }
    public List<SkinDataSO> SkinDatas { get { return skindataList; } }
    public void Init()
    {
        GameObject root = GameObject.Find("@DataManager");
        if (root == null)
        {
            root = Managers.Resource.Instantiate("UI/DataManager");
            GameObject.DontDestroyOnLoad(root);
        }
    }
}

