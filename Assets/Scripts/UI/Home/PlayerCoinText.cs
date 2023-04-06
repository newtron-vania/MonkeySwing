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
        Debug.Log($"Player : {GameManagerEx.Instance.player}");
        Debug.Log($"player money : {GameManagerEx.Instance.player.Money}");

        BananaCounttext.text = GameManagerEx.Instance.player.Money.ToString();
    }
}
