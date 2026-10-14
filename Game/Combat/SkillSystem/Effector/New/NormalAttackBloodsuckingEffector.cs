using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.NormalAttackBloodsucking)]
    public class NormalAttackBloodsuckingEffector : BaseEffector
    {

        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            NormalAttackBloodsucking(skillOwner, ref damageInfo, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Debug.LogError("Combat 吸血效果触发时机不正确");
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Debug.LogError("Combat 吸血效果触发时机不正确");
        }

        void NormalAttackBloodsucking(EntityInfo skillOwner, ref DamageInfo damageInfo, List<int> param)
        {
            int percentage = param[0];// 吸血百分比
            long value = CombatUtility.CeilToLong(damageInfo.FinalDamageValue * percentage * 0.01d);
            skillOwner.Entity.AttributeSet.AddHP(skillOwner.Entity, value);
        }
    }
}