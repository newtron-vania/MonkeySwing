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
    }


    void FixedUpdate()
    {
        float moveX = 0;
#if (UNITY_EDITOR)
        if (Input.GetMouseButton(0))
        {
            moveX = Input.GetAxisRaw("Mouse X");
        }
#endif
        Vector2 nextMove = rigid.position + new Vector2(moveX * slideSpeed * Time.fixedDeltaTime, 0);
        if (nextMove.x < -2.5f)
            nextMove.x = -2.5f;
        else if (nextMove.x > 2.5f)
            nextMove.x = 2.5f;
        rigid.MovePosition(nextMove);

    }
}
