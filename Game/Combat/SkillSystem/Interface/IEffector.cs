using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IEffector
    {
        void Execute(SkillExcutor excutor, DTSkill_effect.ETime time, EntityInfo attacker, EntityInfo target, ref DamageInfo damageInfo, List<int> param);
    }
}