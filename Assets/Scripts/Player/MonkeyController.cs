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

    public float invincibilityTime = 2f;
    public bool isDamaged = false;

    Coroutine InvinvibleCoroutine;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(LooseWeight());
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
        if(!isDamaged && (collision.tag == "Enemy" || collision.tag == "Snake"))
            BeDamaged();
        else if(collision.gameObject.tag == "LineMid" || collision.gameObject.tag == "LineTop")
        {
            Distance.distance += 5;
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
