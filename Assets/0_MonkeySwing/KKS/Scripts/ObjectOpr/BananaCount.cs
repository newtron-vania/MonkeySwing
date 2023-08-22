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
        // bestbananacount�� ������ �����ص�, ���� ui���� �ȸ���.
        if ((int)bananacount >= bestbananacount){
            bestbananacount = bananacount;
            bananaCountUpEvent.Invoke();
        }
        GetComponent<TextMeshProUGUI>().text = ((int)bananacount).ToString();
    }
}
