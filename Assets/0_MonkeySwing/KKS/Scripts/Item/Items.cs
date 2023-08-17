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
    //Items 추상 메소드
    protected abstract void ItemEvent();

    //Items 가상 메소드. 아이템 중 예외적인 방식이 존재할 가능성이 있음
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //ItemZone과 충돌 시 이벤트 실행 후, 파괴
        if(collision.tag == "ItemZone")
        {
            ItemEvent();
            Managers.Resource.Destroy(this.gameObject);
        }
    }

}
