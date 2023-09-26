using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkSkill :  BaseSkill
{
    public int GetSkillId()
    {
        return 101;
    }
    public void UseDeadSkill()
    {
        GameManagerEx.Instance.GameOver();
    }

    public void UseStartSkill()
    {

    }

    public void UseTermSkill(int score)
    {
        if(score >0 && score%100 == 0)
        {
            GetItem();
        }
    }

    private void GetItem()
    {
        GameObject go = null;
        if (GameManagerEx.Instance.monkey.transform.GetComponentInChildren<MilkField>() != null)
            go = GameManagerEx.Instance.monkey.transform.GetComponentInChildren<MilkField>().gameObject;
        else
            go = Managers.Resource.Instantiate("Item/MilkField", GameManagerEx.Instance.monkey.transform.position, GameManagerEx.Instance.monkey.transform);
        go.GetComponent<MilkField>().ResetTime();
    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Term;
    }
}
