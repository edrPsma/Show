using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface ITriggerCondition
    {
        bool Decide(SkillInfo skillInfo, SkillExcutor excutor, int triggerValue);
    }
}