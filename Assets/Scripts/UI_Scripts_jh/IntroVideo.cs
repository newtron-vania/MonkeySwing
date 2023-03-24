using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    private VideoPlayer vid;
    [SerializeField]
    private GameObject TouchToStartUI;

    private void Start()
    {
        vid = GetComponent<VideoPlayer>();
        // !PlayerPrefs.HasKey("tutorial_played")
        if(!PlayerPrefs.HasKey("tutorial_played"))
        {
            vid.Play();
            Tutorial_Start();
            Debug.Log("video started");
            PlayerPrefs.SetInt("tutorial_played",1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("already played");
            TouchToStartUI.SetActive(true);
            transform.parent.gameObject.SetActive(false);
            return;
        }
    }

    private void Update()
    {
        if (vid.isPaused)
        {
            TouchToStartUI.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void Tutorial_Start()
    {
        Debug.Log("video Tutorial_Start");
        StartCoroutine(Video_end());
    }

    private IEnumerator Video_end()
    {
        yield return new WaitForSeconds(0.1f);
        //yield return null;
        if(vid.isPlaying == false){
            Debug.Log("video over");
            transform.parent.gameObject.SetActive(false);
        }
    }

    
/*
    void Update()
    {
        vid.Play();
        if(vid.isPlaying == false){
            Debug.Log("video over");
            transform.parent.gameObject.SetActive(false);
        }
    }*/
 /*
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("video over");
        transform.parent.gameObject.SetActive(false);
    }
    */
}
