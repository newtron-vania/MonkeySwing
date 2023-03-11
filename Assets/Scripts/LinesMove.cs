using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesMove : MonoBehaviour
{
<<<<<<< HEAD
    public float speed = 2;
=======
    public float speed = 2f;
>>>>>>> PlayerMove_Test
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        // Debug.Log("movemove");
    }

    
}
