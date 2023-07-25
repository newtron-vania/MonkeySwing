using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    BackgroundChanger backgroundChanger;

    
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
        backgroundChanger = this.GetComponent<BackgroundChanger>();
        StartCoroutine("ScrollBackground");
        GameManagerEx.Instance.distance.distanceEvent -= ChangeDay;
        GameManagerEx.Instance.distance.distanceEvent += ChangeDay;

    }

    private void ChangeDay(int dist)
    {
        if (dist > 0 && dist % 100 == 0)
            IsDay = !IsDay;
    }

    IEnumerator ScrollBackground()
    {
        backgroundChanger.SetBackgroundImg(ScrollBackgroundList[0].GetComponent<SpriteRenderer>());
        backgroundChanger.SetBackgroundImg(ScrollBackgroundList[1].GetComponent<SpriteRenderer>());
        while (true)
        {
            foreach(Transform background in ScrollBackgroundList)
            {
                background.position += Vector3.up * scrollSpeed * Time.fixedDeltaTime;
                if (background.position.y >= endPoint.position.y)
                {
                    backgroundChanger.SetBackgroundImg(background.GetComponent<SpriteRenderer>());
                    background.position = startPoint.position;
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
