using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[Serializable]
public class JsonDataArray<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}

public static class DictionaryJsonUtility
{

    /// <summary>
    /// Dictionary�� Json���� �Ľ��ϱ�
    /// </summary>
    /// <typeparam name="TKey">Dictionary Key�� ����</typeparam>
    /// <typeparam name="TValue">Dictionary Value�� ����</typeparam>
    /// <param name="jsonDicData"></param>
    /// <returns></returns>
    public static string ToJson<TKey, TValue>(Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData;
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>();
            dictionaryData.Key = key;
            dictionaryData.Value = jsonDicData[key];
            dataList.Add(dictionaryData);
        }
        JsonDataArray<TKey, TValue> arrayJson = new JsonDataArray<TKey, TValue>();
        arrayJson.data = dataList;

        return JsonUtility.ToJson(arrayJson, pretty);
    }

    /// <summary>
    /// Json Data�� �ٽ� Dictionary�� �Ľ��ϱ�
    /// </summary>
    /// <typeparam name="TKey">Dictionary Key�� ����</typeparam>
    /// <typeparam name="TValue">Dictionary Value�� ����</typeparam>
    /// <param name="jsonData">�Ľ̵Ǿ��� ������</param>
    /// <returns></returns>

    public static Dictionary<TKey, TValue> FromJson<TKey, TValue>(string jsonData)
    {
        JsonDataArray<TKey, TValue> dataList = JsonUtility.FromJson<JsonDataArray<TKey, TValue>>(jsonData);

        Debug.Log(typeof(TKey));
        Debug.Log(typeof(TValue));
        Debug.Log(dataList.data.Count);

        foreach (var data in dataList.data)
        {
            Debug.Log($"{data.Key}({data.Key.GetType()}) : {data.Value}({data.Value.GetType()})");
        }

        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();
        for (int i = 0; i < dataList.data.Count; i++)
        {
            DataDictionary<TKey, TValue> dictionaryData = dataList.data[i];
            returnDictionary[dictionaryData.Key] = dictionaryData.Value;
        }
        return returnDictionary;

    }
}
public static class Extension
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindUIEvent(go, action, type);
    }
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        return Util.FindChild<T>(go, name, recursive);
    }
    public static GameObject FindChild(this GameObject go, string name = null, bool recursive = false)
    {
        return Util.FindChild(go, name, recursive);
    }


}
