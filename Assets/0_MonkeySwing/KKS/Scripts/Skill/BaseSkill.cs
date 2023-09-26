using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Start,
    Term,
    Dead
}

public interface BaseSkill
{
    public abstract int GetSkillId();
    public abstract SkillType GetType();

    public abstract void UseStartSkill();

    public abstract void UseTermSkill(int score);

    public abstract void UseDeadSkill();

}
