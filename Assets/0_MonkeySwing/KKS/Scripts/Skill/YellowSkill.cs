using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSkill :  BaseSkill
{
    public int GetSkillId()
    {
        return 2;
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
        if(score % 100 == 0)
        {
            GetItem();
        }
    }


    private void GetItem()
    {
        GameObject go = null;
        if (GameObject.FindAnyObjectByType<BananaUp>())
        {
            go = GameObject.FindAnyObjectByType<BananaUp>().gameObject;
        }
        else
        {
            go = Managers.Resource.Instantiate("Item/BananaUp");
        }

        go.GetComponent<BananaUp>().ResetTime();
    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Term;
    }
}
