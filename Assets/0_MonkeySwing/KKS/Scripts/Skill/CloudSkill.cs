using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSkill : MonoBehaviour, BaseSkill
{
    public int GetSkillId()
    {
        return 201;
    }
    public void UseDeadSkill()
    {
        GameManagerEx.Instance.GameOver();
    }

    public void UseStartSkill()
    {
        GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().WeightCut[3] = 120;
    }

    public void UseTermSkill(int score)
    {

    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Start;
    }
}
