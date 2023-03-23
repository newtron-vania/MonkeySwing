using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScene : BaseScene
{
    float time = 0;

    public override Define.SceneType _sceneType => throw new System.NotImplementedException();

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if(time > 1.5)
        {
            SceneManager.LoadScene("");
        }
    }
}
