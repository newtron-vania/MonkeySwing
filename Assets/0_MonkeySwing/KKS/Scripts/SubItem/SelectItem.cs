using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectItem : MonoBehaviour
{
    public int mapID;
    [SerializeField]
    Image medalImg;
    [SerializeField]
    Button Playbutton;
    [SerializeField]
    Button MedalButton;
    [SerializeField]
    Button RankButton;
    [SerializeField]
    TextMeshProUGUI mapIdText;

    [SerializeField]
    TextMeshProUGUI scoreText;


    [SerializeField]
    Transform rankUI;
    [SerializeField]
    Transform medalUI;

    // Start is called before the first frame update
    void Start()
    {
        
        mapIdText.text = mapID.ToString();

        rankUI = FindObjectOfType<RankUI>(true).transform;
        medalUI = FindObjectOfType<MedalUI>(true).transform;

        int score = GameManagerEx.Instance.scoreData.GetScore(mapID);
        Debug.Log($"{mapID} score : {score}");
        medalImg.sprite = Managers.Resource.LoadSprite(Managers.Data.GetMedalSpritePath(score, mapID));

        scoreText.text = score.ToString();
        RankButton.onClick.AddListener(() => ShowRankUI());
        Playbutton.onClick.AddListener(() => StartMap());
        MedalButton.onClick.AddListener(() => ShowMedals());
    }



    private void StartMap()
    {
        GameManagerEx.Instance.mapID = mapID;
        LoadingScene.LoadScene(Define.SceneType.MainScene);
    }

    private void ShowRankUI()
    {
        rankUI.GetComponent<RankUI>().mapid = mapID;
        rankUI.gameObject.SetActive(true);
    }

    private void ShowMedals()
    {
        //Show MedalUI with ID
        medalUI.GetComponent<MedalUI>().mapid = mapID;
        medalUI.gameObject.SetActive(true);
    }
}
