using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    private void OnEnable()
    {

    }

    private void Start()
    {

    }
    private void OnDestroy()
    {

    }
    public void Destroy()
    {

        Managers.Resource.Destroy(this.gameObject);
    }
}
