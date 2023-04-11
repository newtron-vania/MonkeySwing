using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Items
{
    [SerializeField]
    private GameObject MagnetFieldPrefab;
    protected override void ItemEvent()
    {
        if (GameManagerEx.Instance.monkey.transform.GetComponentInChildren<MagnetField>(true) != null)
            GameManagerEx.Instance.monkey.transform.GetComponentInChildren<MagnetField>().ResetTime();
        else
            Managers.Resource.Instantiate(MagnetFieldPrefab, GameManagerEx.Instance.monkey.transform.position, GameManagerEx.Instance.monkey.transform);
        Managers.Sound.Play("Magnet");
    }
}
