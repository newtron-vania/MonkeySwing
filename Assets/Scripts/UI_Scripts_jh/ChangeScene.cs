using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene
{
    public void ChangeSceneBtn(string TargetScene)
    {
        //Time.timeScale = 1;
        //SceneManager.LoadScene(TargetScene);
        
        switch (TargetScene)
        {
            case "PlayerMoveTestScene2":
            // ���� ���̸� ���� �ʿ�
                SceneManager.LoadScene(TargetScene);
                Time.timeScale = 1;
                break;
            case "Home":
                SceneManager.LoadScene(TargetScene);

                break;
        }
        
    }
}
