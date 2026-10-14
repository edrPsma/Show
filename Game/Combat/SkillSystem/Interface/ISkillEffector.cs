using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Combat
{
    public interface IReleaseSkillEffector
    {
        void TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param);
    }

    public interface ITakeDamageBeforeEffector
    {
        void TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param);
    }

    public interface ITakeDamageAfterEffector
    {
        void TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param);
    }

    public interface ISkillEffector : IReleaseSkillEffector, ITakeDamageBeforeEffector, ITakeDamageAfterEffector
    {
        void Excute(SkillExcutor excutor, EntityInfo skillOwner, float deltaTime, List<int> param);
    }
}