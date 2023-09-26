using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputManager
{
    public Action<Define.TouchEvent> TouchAction = null;
    Vector2 touchPos;

    bool _pressed = false;
    float _pressedTime = 0;

    public void Init()
    {
        SetTouchEffect();
    }
    public void OnUpdate()
    {

        // UI 클릭 여부
        //반드시 EventSystem이 Scene상에 존재해야 함
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;




        if (TouchAction != null)
        {
#if (UNITY_EDITOR)
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    //Debug.Log("Touch Began");
                    TouchAction.Invoke(Define.TouchEvent.Began);
                    _pressedTime = Time.time;
                }
                //Debug.Log("Touch Moved");
                TouchAction.Invoke(Define.TouchEvent.Moved);
                _pressed = true;
            }
#elif (UNITY_ANDROID || UNITY_IOS)
            //입력 확인
            if (Input.touchCount > 0)
            {
                //터치 시작 이벤트
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Debug.Log("Touch Began");
                    TouchAction.Invoke(Define.TouchEvent.Began);
                    _pressedTime = Time.time;
                }
                //터치 후 드래그 이벤트
                if(Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    TouchAction.Invoke(Define.TouchEvent.Moved);
                }
                touchPos = Input.GetTouch(0).position;
                Debug.Log(touchPos);
                _pressed = true;
            }
#endif
            else
            {
                //터치가 끝난 상태
                if (_pressed)
                {
                    //터치한 시간이 지정된 시간보다 짧은 경우, 터치 이벤트 실행
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        TouchAction.Invoke(Define.TouchEvent.Touched);
                    }
                        
                }
                //터치 확인 초기화
                _pressed = false;
                //터치 시간 초기화
                _pressedTime = 0;
            }
        }
    }

    private void SetTouchEffect()
    {
        TouchAction -= CreateTouchEffect;
        TouchAction += CreateTouchEffect;
    }

    private void CreateTouchEffect(Define.TouchEvent touchEvent)
    {
        if(touchEvent == Define.TouchEvent.Touched)
        {
#if (UNITY_EDITOR)
            Vector3 rPosition = Input.mousePosition;

#elif (UNITY_ANDROID || UNITY_IOS)
            Debug.Log("TouchEvent Start!");
            Debug.Log($"touche vector : {touchPos}");
            Vector3 rPosition = touchPos;
            
#endif

            Managers.Resource.Instantiate("UI/Effect/TouchEffect", rPosition, GameObject.FindWithTag("Canvas").transform);
        }
    }

    public void Clear()
    {
        TouchAction = null;
        SetTouchEffect();
    }
}
