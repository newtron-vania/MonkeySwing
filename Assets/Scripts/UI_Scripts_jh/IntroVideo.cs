using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    private VideoPlayer vid;

    private void Awake() {
        PlayerPrefs.SetInt("tutorial_played", PlayerPrefs.GetInt("tutorial_played", 0));
        vid = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("tutorial_played", 0); // 테스트를 위해 추가 (항상 재생되도록 함)
        // !PlayerPrefs.HasKey("tutorial_played")
        if(PlayerPrefs.GetInt("tutorial_played") == 0)
        {
            PlayerPrefs.SetInt("tutorial_played", 0);
            vid.Play();
            Tutorial_Start();
            Debug.Log("video started");
            PlayerPrefs.SetInt("tutorial_played",1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("tutorial_played") == 1)
        {
            Debug.Log("already played");
            transform.parent.gameObject.SetActive(false);
            return;
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
