using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour
{

    protected Rigidbody2D rigid;

    private void OnEnable()
    {
        rigid = this.GetComponent<Rigidbody2D>();
    }
    //Items �߻� �޼ҵ�
    protected abstract void ItemEvent();

    //Items ���� �޼ҵ�. ������ �� �������� ����� ������ ���ɼ��� ����
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //ItemZone�� �浹 �� �̺�Ʈ ���� ��, �ı�
        if(collision.tag == "ItemZone")
        {
            ItemEvent();
            Managers.Resource.Destroy(this.gameObject);
        }
    }

}
