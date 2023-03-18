using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMaterial : MonoBehaviour
{
    [SerializeField]
    float downForce = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Monkey")
        {
            Rigidbody2D monkeyRigid = collision.gameObject.GetComponent<Rigidbody2D>();
            monkeyRigid.velocity = monkeyRigid.velocity.normalized* downForce;
        }
    }
}
