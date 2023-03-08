using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BananaCount : MonoBehaviour
{
    public static int bananacount = 0;
    public static int bestbananacount = 0;

    // Update is called once per frame
    void Update()
    {
        // bestbananacount는 변수만 저장해둠, 아직 ui따로 안만듦.
        if (bananacount >= bestbananacount){
            bestbananacount = bananacount;
        }
        GetComponent<TextMeshProUGUI>().text = "Banana Count : " + bananacount.ToString();
    }
}
