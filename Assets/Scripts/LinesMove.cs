using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesMove : MonoBehaviour
{
    public float speed = 2f;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        // Debug.Log("movemove");
    }

    
}
