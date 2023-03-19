using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    Rigidbody2D rigid;
    private int health;
    public int Health
    {
        get { return health; }
        set { 
            health = value; 
            if(health <= 0)
            {
                GameManagerEx.Instance.GameOver();
            }
        }
    }

    public float maxVelocityForce = 10f;


    public float gravity = 1f;
    public int weight = 50;

    public float invincibilityTime = 2f;
    public bool isDamaged = false;

    Coroutine InvinvibleCoroutine;
    void Start()
    {
        health = 3;
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(LooseWeight());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //VelocityCheck();
        CheckLinearDrag();
        CheckGravity();
    }

    void CheckVelocity()
    {
        maxVelocityForce = (-12 / 90f * weight) + 49 / 3f;
        if (rigid.velocity.magnitude > maxVelocityForce)
        {
            Vector3 moveVec = rigid.velocity.normalized;
            rigid.velocity = moveVec * maxVelocityForce;
        }
    }

    void CheckLinearDrag()
    {
        rigid.drag = (float)(1 / 30f * (weight - 10) + 1);
    }

    void CheckGravity()
    {
        rigid.gravityScale = Mathf.Max(1 + transform.localPosition.y, 1);
        gravity = rigid.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDamaged && (collision.tag == "Enemy" || collision.tag == "Snake"))
            BeDamaged();
        else if(collision.gameObject.tag == "LineMid" || collision.gameObject.tag == "LineTop")
        {
            GameManagerEx.Instance.distance.Dist += 5;
        }
    }
    IEnumerator LooseWeight()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(weight > 10)
                weight -= 1;
        }
        
    }

    public void BeDamaged()
    {
        HeartCount.heartcount -= 1;
        CameraController.CameraShakeEvent(invincibilityTime, 1f);
        StartInvinvible(invincibilityTime);
    }

    public void StartInvinvible(float time)
    {
        StopInvincible();
        InvinvibleCoroutine = StartCoroutine(OnInvincible(time));
    }

    private void StopInvincible()
    {
        if (InvinvibleCoroutine != null)
        {
            StopCoroutine(InvinvibleCoroutine);
        }
    }

    IEnumerator OnInvincible(float time)
    {
        isDamaged = true;
        yield return new WaitForSeconds(time);
        isDamaged = false;
    }
}
