using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public int mapID;
    [SerializeField]
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() => StartMap());
    }


    private void StartMap()
    {
        GameManagerEx.Instance.mapID = mapID;
        LoadingScene.LoadScene(Define.SceneType.MainScene);
    }
}
