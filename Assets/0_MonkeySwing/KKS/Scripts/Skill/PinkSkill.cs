using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkSkill : MonoBehaviour, BaseSkill
{
    public int GetSkillId()
    {
        return 1;
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
                monkey.GetComponent<MonkeyStat>().Hp = 4;
                monkey.SetMonkeyStat();
            };
        }
        else
            GameManagerEx.Instance.monkey.GetComponent<MonkeyStat>().Hp = 4;
    }

    public void UseTermSkill(int score)
    {

    }

    SkillType BaseSkill.GetType()
    {
        return SkillType.Start;
    }
}
