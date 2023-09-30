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
        ItemDataSO itemStat = Managers.Data.itemDict["Magnet"];
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
        //if(skinid == Blue(3))
        if(GameManagerEx.Instance.player.MonkeySkinId == 3)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        Debug.Log($"Destory in {time}");
        ResetTime();
    }
}
