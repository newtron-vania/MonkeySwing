#define __U

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform player;
    Rigidbody2D rigid;


    public float slideSpeed = 2f;
    void Start()
    {
        rigid = player.GetComponent<Rigidbody2D>();
#if(UNITY_EDITOR)
        slideSpeed = PlayerPrefs.GetFloat("slideSpeed", 24f);
        if (slideSpeed < 24f)
            slideSpeed = 24f;
        PlayerPrefs.SetFloat("slideSpeed", slideSpeed);
#elif (UNITY_ANDROID)
        slideSpeed = PlayerPrefs.GetFloat("slideSpeed", 5f);
        if(slideSpeed < 5f)
            slideSpeed = 5f;
        PlayerPrefs.SetFloat("slideSpeed", slideSpeed);

#endif
        Managers.Input.TouchAction -= OnMoved;
        Managers.Input.TouchAction += OnMoved;
    }


    void OnMoved(Define.TouchEvent touchEvent)
    {
        float moveX = 0;

        if (touchEvent == Define.TouchEvent.Moved)
        {
#if (UNITY_EDITOR)
            moveX = Input.GetAxisRaw("Mouse X");
#elif (UNITY_ANDROID || UNITY_IOS)
            moveX = Input.GetTouch(0).deltaPosition.x * 0.05f;
            if (Mathf.Abs(moveX) < 0.1f)
                moveX = 0;
#endif
        }


        Vector2 nextMove = rigid.position + new Vector2(moveX * slideSpeed * Time.fixedDeltaTime, 0);

        if (nextMove.x < -2.5f)
            nextMove.x = -2.5f;
        else if (nextMove.x > 2.5f)
            nextMove.x = 2.5f;
        rigid.MovePosition(nextMove);

    }
}
