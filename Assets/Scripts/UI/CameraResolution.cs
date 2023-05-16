using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraResolution : MonoBehaviour
{
    [SerializeField]
    Camera MainCamera;
    [SerializeField]
    private Camera AfterRenderingCamera;

    private void Start()
    {
        FindCamera();
        SetResolution(); // 초기에 게임 해상도 고정
        //ResolutionFix();
    }

    private void FindCamera()
    {
        if (MainCamera == null || AfterRenderingCamera == null)
        {
            MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            AfterRenderingCamera = GameObject.FindWithTag("AfterRenderingTag").GetComponent<Camera>();
        }
        Object.DontDestroyOnLoad(MainCamera);
        Object.DontDestroyOnLoad(AfterRenderingCamera);
    }
    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        
        int setWidth = 1080; // 사용자 설정 너비
        int setHeight = 1920; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            Debug.Log("기기의 해상도가 더 높음!");
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            MainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            Debug.Log("게임의 해상도가 더 높음!");
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            MainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;
    }


    void ResolutionFix()
    {
        Camera camera = GetComponent<Camera>();

        float targetWidthAspect = 9.0f;
        float targetHeightAspect = 16.0f;


        float targetWidthAspectPort = targetWidthAspect / targetHeightAspect;
        float targetHeightAspectPort = targetHeightAspect / targetWidthAspect;

        float currentWidthAspectPort = (float)Screen.width / (float)Screen.height;
        float currentHeightAspectPort = (float)Screen.height / (float)Screen.width;

        float viewPortW = targetWidthAspectPort / currentWidthAspectPort;
        float viewPortH = targetHeightAspectPort / currentHeightAspectPort;

        if (viewPortH > 1)
            viewPortH = 1;
        if (viewPortW > 1)
            viewPortW = 1;
        camera.rect = new Rect(
            (1 - viewPortW) / 2,
            (1 - viewPortH) / 2,
            viewPortW,
            viewPortH);
    }

    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {

        GL.Clear(true, true, Color.black);

    }
}
