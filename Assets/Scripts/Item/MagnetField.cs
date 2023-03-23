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
            Managers.Resource.Destroy(this.gameObject);
        transform.rotation = Quaternion.identity;
        time += Time.fixedDeltaTime;
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
