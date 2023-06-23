using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Effect Enabled!");
    }

    private void Start()
    {
        Debug.Log("Effect Started!");
    }
    private void OnDestroy()
    {
        Debug.Log("Effect OnDestroyed2");
    }
    public void Destroy()
    {
        Debug.Log("Effect Destroyed!");
        Managers.Resource.Destroy(this.gameObject);
    }
}
