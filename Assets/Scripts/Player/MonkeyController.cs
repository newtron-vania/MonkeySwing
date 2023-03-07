using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    Rigidbody2D rigid;
    public int health = 3;

    public float maxVelocityForce = 10f;
    public float gravity = 1f;
    public int weight = 50;

    public bool isDamaged = false;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        maxVelocityForce = (-12/90f * weight) + 49/3f;
        if(rigid.velocity.magnitude > maxVelocityForce)
        {
            Vector3 moveVec = rigid.velocity.normalized;
            rigid.velocity = moveVec * maxVelocityForce;
        }
        rigid.gravityScale = Mathf.Max(1 +transform.localPosition.y, 1);
        gravity = rigid.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDamaged)
            StartCoroutine(BeDamaged());
        
    }
    IEnumerator LooseWeight()
    {
        weight = 50;
        yield return new WaitForSeconds(1f);
        weight -= 1;
    }

    IEnumerator BeDamaged()
    {
        isDamaged = true;
        health -= 1;
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }
}
