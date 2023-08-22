using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSkill : MonoBehaviour, BaseSkill
{
    private int resurrectionCount = 1;
    public int GetSkillId()
    {
        return 200;
    }
    public void UseDeadSkill()
    {
        if (resurrectionCount > 0)
            Resurrection();
        else
        {
            GameManagerEx.Instance.GameOver();
        }
        
    }
    public void Resurrection()
    {
        resurrectionCount -= 1;
        GetItem();

    }

    private void GetItem()
    {
        GameObject go = null;
        if (GameObject.FindAnyObjectByType<RessuructionItem>())
        {
            go = GameObject.FindAnyObjectByType<RessuructionItem>().gameObject;
        }
        else
        {
            go = Managers.Resource.Instantiate("Item/Ressuruction");
        }
    }
    public void UseStartSkill()
    {

    }

    public void UseTermSkill(int score)
    {

    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Dead;
    }
}
