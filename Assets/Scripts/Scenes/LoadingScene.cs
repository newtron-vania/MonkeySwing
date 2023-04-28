using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    [SerializeField] Image[] LoadingImg;

    const float loadingTime = 3f;

    private void Start()
    {
        Managers.Sound.StopPlayingSound(Define.Sound.Bgm);
        ShowRandomImg();
        GameManagerEx.Instance.GameStart();
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(Define.SceneType sceneType)
    {
        nextScene = sceneType.ToString();
        SceneManager.LoadScene("LoadingScene");
    }



    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f) { progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer); 
                if (progressBar.fillAmount >= op.progress) { timer = 0f; } }
            else
            {
                if (timer < loadingTime)
                    continue;
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f) { op.allowSceneActivation = true; yield break; }
            }
        }
    }

    private void ShowRandomImg()
    {
        int rand = Random.Range(0, LoadingImg.Length);
        LoadingImg[rand].gameObject.SetActive(true);
    }
}
