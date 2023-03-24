using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCoinText : MonoBehaviour
{
    TextMeshProUGUI BananaCounttext;
    private void Start()
    {
        BananaCounttext = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        Debug.Log("Banana Update");
        BananaCounttext.text = GameManagerEx.Instance.player.Money.ToString();
    }
}
