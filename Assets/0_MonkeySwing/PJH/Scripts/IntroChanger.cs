using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroChanger : MonoBehaviour
{
    private Image image;

    [SerializeField] 
    private Sprite[] sprites;

    private int index = 0;

    void Awake() {
        PlayerPrefs.SetInt("tutorial_played", PlayerPrefs.GetInt("tutorial_played", 0));
    }

    void Start()
    {
        PlayerPrefs.SetInt("tutorial_played", 0); // 테스트를 위해 추가한 줄
        // PlayerPrefs.SetInt("tutorial_played", 0); 
        // !PlayerPrefs.HasKey("tutorial_played")
        if(PlayerPrefs.GetInt("tutorial_played") == 0)
        {
            PlayerPrefs.SetInt("tutorial_played", 0);
            image = GetComponent<Image>();
            PlayerPrefs.SetInt("tutorial_played",1);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.GetInt("tutorial_played") == 1)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }
    }

    public void Tutorial_Start()
    {
        StartCoroutine(NextPage());
    }

    private IEnumerator NextPage()
    {
        yield return new WaitForSeconds(0.5f);
        if(sprites.Length == index)
            {
                transform.parent.gameObject.SetActive(false);
                index = 0;
            }
            image.sprite = sprites[index];
            index++;
    }
}
