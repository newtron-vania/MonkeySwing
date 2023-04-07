using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManagerEx.Instance.player = new PlayerData();
        GameManagerEx.Instance.player.MonkeySkinId = 3;
    }
}
