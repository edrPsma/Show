using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.DamageModification)]
    public class DamageModificationEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            DamageModification(excutor, ref damageInfo, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Debug.LogError("Combat 伤害翻倍效果触发时机不正确");
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Debug.LogError("Combat 伤害翻倍效果触发时机不正确");
        }

        void DamageModification(SkillExcutor excutor, ref DamageInfo damageInfo, List<int> param)
        {
            int probability = param[0];
            int minMultiply = param[1];
            int maxMultiply = param[2];

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(excutor.CombatType);

            int randomValue = combatManager.Random.GetRandom(0, 100);
            if (probability >= randomValue)
            {
                int multiple = combatManager.Random.GetRandom(minMultiply, maxMultiply);
                damageInfo.FinalDamageValue = CombatUtility.CeilToLong(damageInfo.FinalDamageValue * multiple * 0.01d);
            }
        }
    }
}