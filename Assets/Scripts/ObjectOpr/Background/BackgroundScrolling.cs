using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField]
    Sprite[] backgrounds;


    float changeBackgroundLimit = 600f;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    Transform endPoint;

    [SerializeField]
    List<Transform> ScrollBackgroundList = new List<Transform>();

    float scrollSpeed = 0.5f;
    float nightColor = 0.5f;

    bool isDay = true;
    public bool IsDay { get { return isDay; } set { if (isDay != value) { isDay = value; StartCoroutine("ChangeNightAndDay");  } } }
   
    
    

    private void Start()
    {
        StartCoroutine("ScrollBackground");
    }

    IEnumerator ScrollBackground()
    {
        int i = 0;
        if (backgrounds.Length < 1)
            yield return null;
        ScrollBackgroundList[0].GetComponent<SpriteRenderer>().sprite = backgrounds[i];
        if (backgrounds.Length >= 2)
            i++;
        ScrollBackgroundList[1].GetComponent<SpriteRenderer>().sprite = backgrounds[i];
        while (true)
        {
            foreach(Transform background in ScrollBackgroundList)
            {
                background.position += Vector3.up * scrollSpeed * Time.fixedDeltaTime;
                if (background.position.y > endPoint.position.y)
                {
                    background.position = startPoint.position;
                    background.GetComponent<SpriteRenderer>().sprite = backgrounds[i];
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ChangeNightAndDay()
    {
        float changeTime = 0f;
        List<SpriteRenderer> backgroundSpriteRenderers = new List<SpriteRenderer>();
        foreach (Transform background in ScrollBackgroundList)
        {
            backgroundSpriteRenderers.Add(background.GetComponent<SpriteRenderer>());
        }

        float diff = (1 - nightColor) * 0.1f;
        if (!isDay)
            diff *= -1;

        while (changeTime < 1f)
        {
            foreach (SpriteRenderer backSR in backgroundSpriteRenderers)
            {
                backSR.color += new Color(1f, 1f, 1f, 0) * diff;
            }
            yield return new WaitForSeconds(0.1f);
            changeTime += 0.1f;
        }
    }
}
