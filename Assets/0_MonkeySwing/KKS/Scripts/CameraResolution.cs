using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraResolution : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Camera _afterRenderingCamera;

    private void Start()
    {
        FindCamera();
        SetResolution(); // 珥덇린??寃뚯엫 ?댁긽??怨좎젙
        //ResolutionFix();
    }

    private void FindCamera()
    {
        if (_mainCamera == null || _afterRenderingCamera == null)
        {
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            _afterRenderingCamera = GameObject.FindWithTag("AfterRenderingCamera").GetComponent<Camera>();
        }
        Object.DontDestroyOnLoad(_mainCamera);
        Object.DontDestroyOnLoad(_afterRenderingCamera);
    }
    /* ?댁긽???ㅼ젙?섎뒗 ?⑥닔 */
    public void SetResolution()
    {
        
        int setWidth = 1080; 
        int setHeight = 1920; 

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height; 

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); 

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) 
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            _mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else 
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); 
            _mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); 
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
