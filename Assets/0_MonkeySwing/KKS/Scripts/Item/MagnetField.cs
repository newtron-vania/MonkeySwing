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
    private void SetItemStat()
    {
        ItemDataSO itemStat = Managers.Data.GetItem("Magnet");
        maxTTL = itemStat.time;
        transform.localScale = Vector3.one * itemStat.size;
    }

    public void ResetTime()
    {
        time = 0f;
    }

    private void OnEnable()
    {
        SetItemStat();
        ResetTime();
    }
    private void OnDestroy()
    {
        Debug.Log($"Destory in {time}");
    }
}
