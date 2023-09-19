using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonkeyController : MonoBehaviour
{

    public Action<int> healthEvent;

    private Rigidbody2D rigid;
    private Animator anime;

    [SerializeField]
    private Sprite[] monkeyFaceMode;
    [SerializeField]
    private SpriteRenderer monkeyFace;
    [SerializeField]
    private List<SpriteRenderer> monkeyBody; // 0 : tail 1 : head

    private MonkeyStat stat;
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            healthEvent?.Invoke(health);
            if (health <= 0)
            {
                skill.UseDeadSkill();
            }
        }
    }

    public float maxVelocityForce = 10f;


    public float gravity = 1f;
    [SerializeField]
    private int weight;

    public int Weight
    {
        get { return weight; }
        set
        {
            weight = value;
            if (weightEvent != null)
                weightEvent.Invoke(weight);
            if (isDamaged)
                return;
            if (weight <= stat.WeightCut[1])
                PlayerState = Define.CharacterState.Hunger;
            else if (weight <= stat.WeightCut[2])
                PlayerState = Define.CharacterState.Normal;
            else
                PlayerState = Define.CharacterState.Full;
        }
    }

    public Action<int> weightEvent;

    Define.CharacterState playerState = Define.CharacterState.Normal;
    Define.CharacterState PlayerState
    {
        get { return playerState; }
        set
        {
            playerState = value;
            switch (playerState)
            {
                case Define.CharacterState.Hunger:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Hunger];
                    rigid.drag = .8f;
                    break;
                case Define.CharacterState.Normal:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Normal];
                    rigid.drag = 2f;
                    break;
                case Define.CharacterState.Full:
                    monkeyFace.sprite = monkeyFaceMode[(int)Define.CharacterState.Full];
                    rigid.drag = 5f;
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


    BaseSkill skill;

    [SerializeField]
    private float damagedTime = 2f;
    private bool isDamaged = false;
    private bool isInvincible = false;



    private void Start()
    {
        skill = Managers.Data.skillDict[GameManagerEx.Instance.player.MonkeySkinId];
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        stat = GetComponent<MonkeyStat>();



        GameManagerEx.Instance.distance.distanceEvent -= skill.UseTermSkill;
        GameManagerEx.Instance.distance.distanceEvent += skill.UseTermSkill;
        skill.UseStartSkill();
        SetMonkeyStat();
        StartCoroutine(LooseWeight());
    }

    public void SetMonkeyStat()
    {
        Health = stat.Hp;
        Weight = stat.Weight;
        GameManagerEx.Instance.makeLines.LineSpeed = stat.Speed;
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
        if (!isInvincible && (collision.tag == "Enemy"))
        {
            PlayerState = Define.CharacterState.Damaged;
        }

        else if (collision.gameObject.tag == "LineMid" || collision.gameObject.tag == "LineTop")
        {
            GameManagerEx.Instance.distance.Dist += 5;
        }
    }
    IEnumerator LooseWeight()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Weight > stat.WeightCut[0])
                Weight -= 1;
        }

    }
    Coroutine Coroutine;
    Coroutine InvinvibleCoroutine;

    public void StartOnDamaged(float damagedTime)
    {
        StopOnDamaged();
        Coroutine = StartCoroutine(OnDamaged(damagedTime));
    }

    private void StopOnDamaged()
    {
        if (Coroutine != null)
        {
            Debug.Log("OnDamaged Coroutine Out!");
            Weight = weight;
            StopCoroutine(Coroutine);
            isDamaged = false;
        }
    }

    public void BeDamaged()
    {
        Managers.Sound.Play("Damaged");
        Coroutine = StartCoroutine(OnDamaged(damagedTime));
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
        Coroutine = StartCoroutine(OnBoost(continuousTime, waitTime));
    }

    private void StopBoost()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Debug.Log("BoostStop");
        }
    }

    IEnumerator OnBoost(float continuousTime, float waitTime)
    {

        StartInvinvible(continuousTime + waitTime);
        anime.Play("Booster");
        Debug.Log("Boost On");
        yield return new WaitForSeconds(continuousTime);
        Debug.Log("Boost Over! Invinvible continue");
        anime.Play("BoostOver");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Boost Over! Invinvible Over!");
        anime.Play("Normal");
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