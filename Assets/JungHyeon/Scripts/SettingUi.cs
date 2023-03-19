using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUi : MonoBehaviour
{
        public void OnClickExitButton()
    {
        Debug.Log("click Exit");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   
        Application.Quit();
#endif
    }
}
