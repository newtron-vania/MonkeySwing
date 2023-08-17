using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectItem : MonoBehaviour
{
    public int mapID;
    [SerializeField]
    Button Playbutton;
    [SerializeField]
    Button MedalButton;
    [SerializeField]
    Button RankButton;
    [SerializeField]
    TextMeshProUGUI mapIdText;


    [SerializeField]
    Transform rankUI;

    // Start is called before the first frame update
    void Start()
    {
        
        mapIdText.text = mapID.ToString();

        rankUI = FindObjectOfType<RankUI>().transform;
        RankButton.onClick.AddListener(() => ShowRankUI());
        Playbutton.onClick.AddListener(() => StartMap());

    }



    private void StartMap()
    {
        GameManagerEx.Instance.mapID = mapID;
        LoadingScene.LoadScene(Define.SceneType.MainScene);
    }

    private void ShowRankUI()
    {
        rankUI.gameObject.SetActive(true);
    }

    private void ShowMedals()
    {
        //Show MedalUI with ID
    }
}
