using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSkill :  BaseSkill
{
    public int GetSkillId()
    {
        return 103;
    }
    public void UseDeadSkill()
    {
        GameManagerEx.Instance.GameOver();
    }

    public void UseStartSkill()
    {
        if (GameManagerEx.Instance.monkey == null)
        {
            GameObject.FindAnyObjectByType<MainScene>().monkeySetEvent += (monkey) =>
            {
                monkey.GetComponent<MonkeyStat>().WeightCut[3] = 120;
                monkey.SetMonkeyStat();
            };
        }
        else
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
