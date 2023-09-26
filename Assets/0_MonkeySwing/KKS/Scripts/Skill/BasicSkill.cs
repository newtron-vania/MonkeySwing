using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : BaseSkill
{

    public int GetSkillId()
    {
        return 0;
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

    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Start;
    }

}
