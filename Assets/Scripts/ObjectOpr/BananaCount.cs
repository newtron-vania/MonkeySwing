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
        // bestbananacount�� ������ �����ص�, ���� ui���� �ȸ���.
        if (bananacount >= bestbananacount){
            bestbananacount = bananacount;
        }
        GetComponent<TextMeshProUGUI>().text = "Banana Count : " + bananacount.ToString();
    }
}
