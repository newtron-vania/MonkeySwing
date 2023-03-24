using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeartCount : MonoBehaviour
{
    public static int heartcount = 3;
    public bool is_dead = false;
    public GameObject resultPopup;

    private void OnEnable() {
        // Result_Popup = GameObject.Find("ResultPopup");
        is_dead = false;
        Debug.Log("start sdsd rpop");
        //Heart_Counting();
    }
 
    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("update rpop");
        GetComponent<TextMeshProUGUI>().text = "Heart Count : " + heartcount.ToString();
        //Debug.Log(heartcount);
        //if(heartcount <= 0 & is_dead == false){
        //    is_dead = true;
        //    Time.timeScale = 0;
        //    Result_Popup.SetActive(true);
        //    /*
        //    int now_banana = PlayerPrefs.GetInt("totalbananacount");
        //    PlayerPrefs.SetInt("totalbananacount", now_banana + BananaCount.bananacount);
        //    */
        //}
    }

    /*
        public void Heart_Counting()
        {
            StartCoroutine(Is_dead());
        }

        private IEnumerator Is_dead()
        {
            yield return null;
            Debug.Log("update rpop");
            GetComponent<TextMeshProUGUI>().text = "Heart Count : " + heartcount.ToString();
            // yield return new WaitForSeconds(1f);

            if(heartcount <= 0){
                Result_Popup.SetActive(true);
            }
        }*/
}
