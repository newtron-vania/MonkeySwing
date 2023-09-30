using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Play_HeartBit : MonoBehaviour
{
    [SerializeField]
    private float maxSize = 1.15f;
    [SerializeField]
    private float minSize = 0.8f;

    [SerializeField]
    private float currentSize = 1f;
    [SerializeField]
    private float midValue = 0.05f;

    private bool isUp = true;
    void Start()
    {
        float startSize = (minSize + maxSize) * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp)
        {
            currentSize += (maxSize - currentSize) * midValue;
            if (maxSize - currentSize < 0.01f)
                isUp = !isUp;
        }
        else
        {
            currentSize = Mathf.Max(minSize, currentSize - (maxSize - currentSize) * midValue);
            if (currentSize - minSize < 0.01f)
                isUp = !isUp;
        }

        this.transform.localScale = Vector3.one * currentSize;
    }
}
