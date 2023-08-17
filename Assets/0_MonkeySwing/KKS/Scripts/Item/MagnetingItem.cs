using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetingItem : MonoBehaviour
{
    //자석 효과 적용 유무 변수
    [SerializeField]
    private bool Magneting = false;

    //가속도
    [SerializeField]
    float force = 0.5f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //MagnetField와 충돌하고 아직 자석 효과를 받지 않았을 경우
        if (collision.tag == "MagnetField" && !Magneting)
        {
            //자석 효과 적용 확인
            Magneting = true;
            //목표 대상 설정
            MonkeyController monkey = GameManagerEx.Instance.monkey;
            //자석 효과 시작
            StartCoroutine(MoveToPlayer(monkey));
            Debug.Log("Banana is MagnetField!");
        }
    }

    private void OnDisable()
    {
        Magneting = false;
    }

    IEnumerator MoveToPlayer(MonkeyController monkey)
    {
        //speed<0이면 목표지점에서 멀어지고, speed>0이면 목표지점으로 가깝게 이동한다
        //자석 효과로 잠깐 벗어났다가 순식간에 끌어당기는 시각적 효과를 부여한다.
        float speed = -4f;
        while (true)
        {
            //방향 확인
            Vector3 dirVec = monkey.transform.position - this.transform.position;
            //목표 방향으로 speed만큼 이동
            this.transform.position += dirVec.normalized * speed * Time.fixedDeltaTime;
            //물리효과 적용시점마다 실행
            //Because 확실한 충돌유무 확인을 위해 이동과 동시에 물리효과를 적용한다. 
            yield return new WaitForFixedUpdate();
            //speed가 점점 증가
            speed += force;
        }
    }
}
