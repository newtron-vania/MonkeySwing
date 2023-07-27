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
        SetResolution(); // 珥덇린??寃뚯엫 ?댁긽??怨좎젙
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
    /* ?댁긽???ㅼ젙?섎뒗 ?⑥닔 */
    public void SetResolution()
    {
        
        int setWidth = 1080; // ?ъ슜???ㅼ젙 ?덈퉬
        int setHeight = 1920; // ?ъ슜???ㅼ젙 ?믪씠

        int deviceWidth = Screen.width; // 湲곌린 ?덈퉬 ???
        int deviceHeight = Screen.height; // 湲곌린 ?믪씠 ???

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution ?⑥닔 ?쒕?濡??ъ슜?섍린

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 湲곌린???댁긽??鍮꾧? ????寃쎌슦
        {
            Debug.Log("湲곌린???댁긽?꾧? ???믪쓬!");
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ?덈줈???덈퉬
            MainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ?덈줈??Rect ?곸슜
        }
        else // 寃뚯엫???댁긽??鍮꾧? ????寃쎌슦
        {
            Debug.Log("寃뚯엫???댁긽?꾧? ???믪쓬!");
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ?덈줈???믪씠
            MainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ?덈줈??Rect ?곸슜
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
