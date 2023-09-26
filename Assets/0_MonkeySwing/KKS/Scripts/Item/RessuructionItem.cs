using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessuructionItem : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        GameManagerEx.Instance.GameStop();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        GameManagerEx.Instance.monkey.SetMonkeyStat();
        GameManagerEx.Instance.GameStart();
    }
}
