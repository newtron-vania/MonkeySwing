using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Items
{
    [SerializeField]
    private GameObject MagnetFieldPrefab;
    protected override void ItemEvent()
    {
        if (monkey.transform.GetComponentInChildren<MagnetField>() != null)
            monkey.transform.GetComponentInChildren<MagnetField>().time = 0f;
        else
            Instantiate(MagnetFieldPrefab, monkey.transform.position, Quaternion.identity, monkey.transform);
    }
}
