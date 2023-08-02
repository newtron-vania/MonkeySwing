using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    public float speed = 3f;
    public static bool IsRightDir = true; // true면 오른쪽방향, flase면 왼쪽방향으로 이동

    // Update is called once per frame
    void Update()
    {
        if(IsRightDir){
            transform.position += Vector3.right * speed * Time.deltaTime;
            }
        else{
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Wall"){
            if(IsRightDir){
                IsRightDir = false;
            }
            else{
                IsRightDir = true;
            }
        }
    }
}
