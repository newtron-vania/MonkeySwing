using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButtonImageController : MonoBehaviour
{
    [SerializeField] private GameObject on;
    [SerializeField] private GameObject off;

    public void ChangeImage(bool flag)
    {
        if (flag)
        {
            on.SetActive(true);
            off.SetActive(false);
        }
        else
        {
            on.SetActive(false);
            off.SetActive(true);
        }
    }
}
