using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour
{
    private float maxTTL = 5f;
    public float time = 0f;
    private void FixedUpdate()
    {
        if(time >= maxTTL)
            Object.Destroy(this.gameObject);
        transform.rotation = Quaternion.identity;
        time += Time.fixedDeltaTime;
        Debug.Log(time);
    }
}
