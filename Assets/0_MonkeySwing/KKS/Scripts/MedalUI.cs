using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedalUI : MonoBehaviour
{
    public int mapid;

    [SerializeField]
    List<TextMeshProUGUI> medalText;

    void OnEnable()
    {
        List<int> cutlines = Managers.Data.medalCutDict[mapid].cutline;
        for(int i = 1; i <cutlines.Count; i++)
        {
            medalText[i].text = cutlines[i].ToString();
        }
    }
}
