using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonkeyController : MonoBehaviour
{



    private Rigidbody2D rigid;
    private Animator anime;

    [SerializeField]
    private Sprite[] monkeyFaceMode;
    [SerializeField]
    private SpriteRenderer monkeyFace;
    [SerializeField]
    private List<SpriteRenderer> monkeyBody; // 0 : tail 1 : head

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
            if (weightEvent != null)
                weightEvent.Invoke(weight);
            if (isDamaged)
                return;
            if (weight <= 30)
                PlayerState = Define.CharacterState.Hunger;
            else if (weight <= 80)
                PlayerState = Define.CharacterState.Normal;
            else
                PlayerState = Define.CharacterState.Full;
        } 
    }

    public Action<int> weightEvent;

    Define.CharacterState playerState = Define.CharacterState.Normal;
    Define.CharacterState PlayerState { 
        get { return playerState; } 
        set 
        { 
            playerState = value;
            switch (playerState)
            {
                case Define.CharacterState.Hunger:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Hunger];
                    rigid.drag = .5f;
                    break;
                case Define.CharacterState.Normal:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Normal];
                    rigid.drag = 2f;
                    break;
                case Define.CharacterState.Full:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Full];
                    rigid.drag = 4f;
                    break;
                case Define.CharacterState.Damaged:
                    Debug.Log("isDamaged!");
                    BeDamaged();
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Damaged];
                    isDamaged = true;
                    break;
            }
        } 
    }

    [SerializeField]
    private float damagedTime = 2f;
    private bool isDamaged = false;
    private bool isInvincible = false;
    


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        Health = 3;
        Weight = 50;
        StartCoroutine(LooseWeight());
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //VelocityCheck();
        CheckGravity();
    }

    public List<SpriteRenderer> GetSkin()
    {
        return monkeyBody;
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
        if(!isInvincible && (collision.tag == "Enemy"))
        {
            Debug.Log($"collision name : {collision.name } collision tag : {collision.tag}");
            PlayerState = Define.CharacterState.Damaged;
        }
            
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
    Coroutine OnDamagedCoroutine;
    Coroutine InvinvibleCoroutine;
    Coroutine BoostCoroutine;

    public void StartOnDamaged(float damagedTime)
    {
        StopOnDamaged();
        OnDamagedCoroutine = StartCoroutine(OnDamaged(damagedTime));
    }

    private void StopOnDamaged() 
    {
        if (OnDamagedCoroutine != null)
        {
            Debug.Log("OnDamaged Coroutine Out!");
            isDamaged = false;
            anime.Play("Normal");
            Weight = weight;
            StopCoroutine(OnDamagedCoroutine);
        }
    }

    public void BeDamaged()
    {
        StartCoroutine(OnDamaged(damagedTime));
    }

    IEnumerator OnDamaged(float damagedTime)
    {
        anime.Play("BeDamaged");
        Health -= 1;
        CameraShake(0.4f, 0.5f);
        StartInvinvible(damagedTime);
        yield return new WaitForSeconds(damagedTime);
        Debug.Log("OnDamaged Out!");
        isDamaged = false;
        anime.Play("Normal");
        Weight = weight;
    }

    public void StartBoost(float continuousTime, float waitTime)
    {
        StopBoost();
        BoostCoroutine = StartCoroutine(OnBoost(continuousTime, waitTime));
    }

    private void StopBoost()
    {
        if (BoostCoroutine != null)
        {
            anime.Play("Normal");
            StopCoroutine(BoostCoroutine);
        }
    }

    IEnumerator OnBoost(float continuousTime, float waitTime)
    {
        anime.Play("Booster");
        Debug.Log("Boost!!!");
        StartInvinvible(continuousTime + waitTime);
        yield return new WaitForSeconds(continuousTime);
        Debug.Log("Boost Over! Invinvible continue");
        anime.Play("BoostOver");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Boost Over! Invinvible Over!");
        anime.Play("Normal");
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
            StopOnDamaged();
            StopBoost();
            isInvincible = false;
            Weight = weight;
            StopCoroutine(InvinvibleCoroutine);
        }
    }




    IEnumerator OnInvincible(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
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
