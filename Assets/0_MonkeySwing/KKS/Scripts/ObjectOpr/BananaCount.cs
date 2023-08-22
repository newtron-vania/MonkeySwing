using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BananaCount : MonoBehaviour
{
    public static float bananacount = 0;
    public static float bestbananacount = 0;

    public static Action bananaCountUpEvent;

    // Update is called once per frame
    void Update()
    {
        // bestbananacount는 변수만 저장해둠, 아직 ui따로 안만듦.
        if ((int)bananacount >= bestbananacount){
            bestbananacount = bananacount;
            bananaCountUpEvent.Invoke();
        }
        GetComponent<TextMeshProUGUI>().text = ((int)bananacount).ToString();
    }
}
