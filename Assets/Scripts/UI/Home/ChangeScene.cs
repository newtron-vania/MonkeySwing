using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneBtn()
    {
        switch (this.gameObject.name)
        {
            case "GameStartBtn":
            // 여기 씬이름 수정 필요
                SceneManager.LoadScene("PlayerMoveTestScene");
                break;
            case "GoHomeBtn":
                SceneManager.LoadScene("Home");
                break;

        }
    }
}
