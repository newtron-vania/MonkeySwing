using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBtn : MonoBehaviour
{
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();

        button.onClick.AddListener(() => GooglePlayManager.Instance.ShowBestScoreLeaderboardUI());

    }


}
