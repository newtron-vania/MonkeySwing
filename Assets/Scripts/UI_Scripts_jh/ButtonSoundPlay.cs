using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundPlay : MonoBehaviour
{
    public void ButtonPlay()
    {
        Managers.Sound.Play("ButtonConfirm");
        GameManagerEx.Instance.GameStop();
    }

    public void PlayToStartPlay()
    {
        Managers.Sound.Play("GameStart");
    }

    public void ButtonEquit()
    {
        Managers.Sound.Play("EquitButton");
    }

    public void SelectMonkey()
    {
        Managers.Sound.Play("MonkeySwing");
    }

}
