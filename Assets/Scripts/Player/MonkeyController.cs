using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anime;
    CameraController camera;
    private int health;
    public int Health
    {
        get { return health; }
        set { 
            health = value;
            HeartCount.heartcount = health;
            if (health <= 0)
            {
                GameManagerEx.Instance.GameOver();
            }
        }
    }

    public float maxVelocityForce = 10f;


    public float gravity = 1f;
    [SerializeField]
    private int weight;
    public int Weight { 
        get { return weight; } 
        set 
        { 
            weight = value;
            anime.SetInteger("weight", weight);
        } 
    }
    [SerializeField]
    private float invincibilityTime = 2f;
    private bool isDamaged = false;
    


    void Start()
    {
        health = 3;
        weight = 50;
        camera = GetComponent<CameraController>();
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
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
        maxVelocityForce = (-12 / 90f * Weight) + 49 / 3f;
        if (rigid.velocity.magnitude > maxVelocityForce)
        {
            Vector3 moveVec = rigid.velocity.normalized;
            rigid.velocity = moveVec * maxVelocityForce;
        }
    }

    void CheckLinearDrag()
    {
        rigid.drag = (float)(1 / 30f * (Weight - 10) + 1);
    }

    void CheckGravity()
    {
        rigid.gravityScale = Mathf.Max(1 + transform.localPosition.y, 1);
        gravity = rigid.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isDamaged && (collision.tag == "Enemy"))
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
            if(Weight > 10)
                Weight -= 1;
        }
        
    }

    Coroutine InvinvibleCoroutine;
    public void BeDamaged()
    {
        Health -= 1;
        CameraShake(0.4f, 0.5f);
        StartDamagedAnime();
        StartInvinvible(invincibilityTime);
        Invoke("EndDamagedAnime", invincibilityTime);
    }

    public void StartBoost(float continuousTime, float waitTime)
    {
        Invoke("StartDamagedAnime", continuousTime);
        Invoke("EndDamagedAnime", continuousTime + waitTime);
        StartInvinvible(continuousTime+ waitTime);
    }

    public void StartDamagedAnime()
    {
        anime.SetBool("isDamaged", true);
    }
    public void EndDamagedAnime()
    {
        anime.SetBool("isDamaged", false);
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

    public void CameraShake(float time, float force)
    {
        StartCoroutine(StartShake(time, force));
    }

    IEnumerator StartShake(float maxTime, float force)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        float shakeTerm = 0.1f;
        float time = 0;
        float basicForce = 0.1f * force;
        while (time <= maxTime)
        {
            Vector3 nextCameraPos = cameraPos;
            float randX = UnityEngine.Random.Range(-basicForce, basicForce);
            float randY = UnityEngine.Random.Range(-basicForce, basicForce);
            Vector3 nextMove = new Vector3(randX, randY, 0);
            Camera.main.transform.position = nextCameraPos + nextMove;
            yield return new WaitForSeconds(shakeTerm);
            time += shakeTerm;
        }
        Camera.main.transform.position = cameraPos;
    }
}
