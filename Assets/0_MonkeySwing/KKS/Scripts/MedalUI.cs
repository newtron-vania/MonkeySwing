using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedalUI : MonoBehaviour
{
    public int _mapid;

    [SerializeField]
    private List<TextMeshProUGUI> _medalText;

    void OnEnable()
    {
        List<int> cutlines = Managers.Data.medalCutDict[_mapid].cutline;
        for(int i = 0; i <cutlines.Count; i++)
        {
            _medalText[i].text = cutlines[i].ToString();
        }
    }
}
