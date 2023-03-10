using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour
{
    public float maxTTL = 5f;
    private float time = 0f;
    private void FixedUpdate()
    {
        if(time >= maxTTL)
            Object.Destroy(this.gameObject);
        transform.rotation = Quaternion.identity;
        time += Time.fixedDeltaTime;
        Debug.Log(Time.fixedDeltaTime);
        Debug.Log(time);
    }

    public void ResetTime()
    {
        time = 0f;
    }

    private void OnDestroy()
    {
        Debug.Log($"Destory in {time}");
    }
}
