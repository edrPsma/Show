using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class SkillRuleAttribute : ReflectionAttribute
    {
        public SkillRuleAttribute(ESkillRuleType ruleType) : base(typeof(ISkillExcutor), ruleType, 100)
        {
        }
    }
}