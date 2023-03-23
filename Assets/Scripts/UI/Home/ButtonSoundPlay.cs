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
}
