using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => GoToSelect());
    }


    private void GoToSelect()
    {
        GameManagerEx.Instance.currentCoin = GameManagerEx.Instance.player.Money;
        SceneManager.LoadScene("SelectScene");
    }
}
