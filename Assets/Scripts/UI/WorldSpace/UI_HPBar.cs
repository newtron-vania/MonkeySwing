using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_HPBar : UI_Base
{
    //Stat _stat;
    enum GameObjects
    {
        HPBar,
    }


    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        //_stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        //ĳ���͸��� Ű�� �ٸ��� ������ ���� y���� ��������� �Ѵ�.
        //�� ĳ������ �ݶ��̴��� ã�Ƴ��� y����ŭ ���̸� �÷��ش�.
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y + 1);
        transform.rotation = Camera.main.transform.rotation;

        //float ratio = _stat.HP / (float)_stat.MaxHP;
        //setHpRatio(ratio);
    }

    public void setHpRatio(float ratio)
    {
        if (ratio < 0)
            ratio = 0;
        if (ratio > 1)
            ratio = 1;
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
