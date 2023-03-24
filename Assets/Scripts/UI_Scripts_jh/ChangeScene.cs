using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene
{
    public void ChangeSceneBtn(string name)
    {
        SceneManager.LoadScene(name);
    }
}
