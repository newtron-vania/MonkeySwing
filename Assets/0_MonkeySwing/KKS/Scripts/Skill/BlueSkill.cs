using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSkill :  BaseSkill
{
    public int GetSkillId()
    {
        return 3;
    }
    public void UseDeadSkill()
    {
        GameManagerEx.Instance.GameOver();
    }

    public void UseStartSkill()
    {
        Managers.Data.itemDict["Magnet"].size *= 1.2f;
    }

    public void UseTermSkill(int score)
    {

    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Start;
    }
}
