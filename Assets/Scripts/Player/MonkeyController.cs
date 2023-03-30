using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    enum CharacterState
    {
        Hunger,
        Normal,
        Full,
        Damaged,
    }

    private Rigidbody2D rigid;
    private Animator anime;

    [SerializeField]
    private Sprite[] monkeyFaceMode;
    [SerializeField]
    private SpriteRenderer monkeyFace;

    private SpriteRenderer[] monkeyBody;

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
            if (isDamaged)
                return;
            if (weight <= 20)
                PlayerState = CharacterState.Hunger;
            else if (weight < 80)
                PlayerState = CharacterState.Normal;
            else
                PlayerState = CharacterState.Full;
        } 
    }

    CharacterState playerState = CharacterState.Normal;
    CharacterState PlayerState { 
        get { return playerState; } 
        set 
        { 
            playerState = value;
            switch (playerState)
            {
                case CharacterState.Hunger:
                    anime.Play("HungerFace");
                    rigid.drag = .5f;
                    break;
                case CharacterState.Normal:
                    anime.Play("NormalFace");
                    rigid.drag = 2f;
                    break;
                case CharacterState.Full:
                    anime.Play("FullFace");
                    rigid.drag = 4f;
                    break;
                case CharacterState.Damaged:
                    Debug.Log("isDamaged!");
                    BeDamaged();
                    anime.Play("BeDamaged");
                    isDamaged = true;
                    break;
            }
        } 
    }

    [SerializeField]
    private float damagedTime = 2f;
    private bool isDamaged = false;
    private bool isInvincible = false;
    


    void Start()
    {
        health = 3;
        weight = 50;
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        StartCoroutine(LooseWeight());

        monkeyBody = transform.GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //VelocityCheck();

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
        if(!isInvincible && (collision.tag == "Enemy"))
            PlayerState = CharacterState.Damaged;
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
    Coroutine BoostCoroutine;
    public void BeDamaged()
    {
        StartCoroutine(OnDamaged(damagedTime));
    }

    IEnumerator OnDamaged(float damagedTime)
    {
        Health -= 1;
        CameraShake(0.4f, 0.5f);
        StartInvinvible(damagedTime);
        while (isInvincible)
        {
            yield return new WaitForFixedUpdate();
        }
        isDamaged = false;
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
            foreach(SpriteRenderer sprite in monkeyBody)
            {
                SetMonkeyColorAlpha(new Color(1f, 1f, 1f, 1f));
            }
            StopCoroutine(BoostCoroutine);
        }
    }

    void SetMonkeyColorAlpha(Color color)
    {
        foreach (SpriteRenderer sprite in monkeyBody)
        {
            sprite.color = color;
        }
    }

    IEnumerator OnBoost(float continuousTime, float waitTime)
    {
        StartInvinvible(continuousTime + waitTime);
        yield return new WaitForSeconds(continuousTime);
        SetMonkeyColorAlpha(new Color(1f, 1f, 1f, 0.5f));
        Debug.Log($"Face Color Setting : { monkeyFace.color}");
        yield return new WaitForSeconds(waitTime);
        SetMonkeyColorAlpha(new Color(1f, 1f, 1f, 1f));
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
            isInvincible = false;
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
