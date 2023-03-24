using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    public void ShowAds()
    {
        GiveMulCoin();
    }

    private void GiveMulCoin()
    {
        BananaCount.bananacount *= 2;
    }
}
