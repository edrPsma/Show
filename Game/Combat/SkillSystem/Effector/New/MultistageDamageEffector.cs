using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.MultistageDamage)]
    public class MultistageDamageEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Debug.LogError("Combat 多段伤害效果触发时机不正确");
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Debug.LogError("Combat 多段伤害效果触发时机不正确");
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            MultistageDamage(excutor, param);
        }

        void MultistageDamage(SkillExcutor excutor, List<int> param)
        {
            excutor.Duration = param[0] * 0.001f;
            excutor.Interval = param[1] * 0.001f;
        }
    }
}