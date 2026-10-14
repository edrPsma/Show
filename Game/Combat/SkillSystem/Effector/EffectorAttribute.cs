using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    public class EffectorAttribute : ReflectionAttribute
    {
        public EffectorAttribute(EEffectType impactorType) : base(typeof(ISkillEffector), impactorType, 100)
        {
        }
    }
}