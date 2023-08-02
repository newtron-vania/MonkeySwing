using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarSizeFix : MonoBehaviour
{
    Scrollbar scrollbar;
    [SerializeField]
    float handleSize = 0f;

    private void OnEnable()
    {
        scrollbar = transform.GetComponent<Scrollbar>();
        StartCoroutine(FixSizeCoroutine(0f));
    }

    public void FixSize(float num)
    {
        scrollbar.size = num;
        Debug.Log(scrollbar.size);
    }

    IEnumerator FixSizeCoroutine(float num)
    {
        yield return null;
        Debug.Log("Resize handle");
        FixSize(num);
    }
}
