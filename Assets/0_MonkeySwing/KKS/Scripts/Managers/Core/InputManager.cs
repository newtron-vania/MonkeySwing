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

        // UI Ŭ�� ����
        //�ݵ�� EventSystem�� Scene�� �����ؾ� ��
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;




        if (TouchAction != null)
        {
#if (UNITY_EDITOR)
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    Debug.Log("Touch Began");
                    TouchAction.Invoke(Define.TouchEvent.Began);
                    _pressedTime = Time.time;
                }
                Debug.Log("Touch Moved");
                TouchAction.Invoke(Define.TouchEvent.Moved);
                _pressed = true;
            }
#elif (UNITY_ANDROID || UNITY_IOS)
            //�Է� Ȯ��
            if (Input.touchCount > 0)
            {
                //��ġ ���� �̺�Ʈ
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Debug.Log("Touch Began");
                    TouchAction.Invoke(Define.TouchEvent.Began);
                    _pressedTime = Time.time;
                }
                //��ġ �� �巡�� �̺�Ʈ
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
                //��ġ�� ���� ����
                if (_pressed)
                {
                    //��ġ�� �ð��� ������ �ð����� ª�� ���, ��ġ �̺�Ʈ ����
                    if (Time.time < _pressedTime + 0.2f)
                    {
                        TouchAction.Invoke(Define.TouchEvent.Touched);
                    }
                        
                }
                //��ġ Ȯ�� �ʱ�ȭ
                _pressed = false;
                //��ġ �ð� �ʱ�ȭ
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