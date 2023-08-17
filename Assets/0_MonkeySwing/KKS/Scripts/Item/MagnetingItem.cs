using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetingItem : MonoBehaviour
{
    //�ڼ� ȿ�� ���� ���� ����
    [SerializeField]
    private bool Magneting = false;

    //���ӵ�
    [SerializeField]
    float force = 0.5f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //MagnetField�� �浹�ϰ� ���� �ڼ� ȿ���� ���� �ʾ��� ���
        if (collision.tag == "MagnetField" && !Magneting)
        {
            //�ڼ� ȿ�� ���� Ȯ��
            Magneting = true;
            //��ǥ ��� ����
            MonkeyController monkey = GameManagerEx.Instance.monkey;
            //�ڼ� ȿ�� ����
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
        //speed<0�̸� ��ǥ�������� �־�����, speed>0�̸� ��ǥ�������� ������ �̵��Ѵ�
        //�ڼ� ȿ���� ��� ����ٰ� ���İ��� ������� �ð��� ȿ���� �ο��Ѵ�.
        float speed = -4f;
        while (true)
        {
            //���� Ȯ��
            Vector3 dirVec = monkey.transform.position - this.transform.position;
            //��ǥ �������� speed��ŭ �̵�
            this.transform.position += dirVec.normalized * speed * Time.fixedDeltaTime;
            //����ȿ�� ����������� ����
            //Because Ȯ���� �浹���� Ȯ���� ���� �̵��� ���ÿ� ����ȿ���� �����Ѵ�. 
            yield return new WaitForFixedUpdate();
            //speed�� ���� ����
            speed += force;
        }
    }
}
