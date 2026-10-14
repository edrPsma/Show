using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Cure)]
    public class CureEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Cure(excutor, skillOwner, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Cure(excutor, skillOwner, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Cure(excutor, skillOwner, param);
        }

        void Cure(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            int targetValue = param[0];
            int fixedValue = param[1];
            int percent = param[2];// 百分比

            // 自身
            if (targetValue == 0)
            {
                var hp = skillOwner.Entity.AttributeSet.HPAttribute;
                long addValue = CombatUtility.CeilToLong(fixedValue + excutor.SkillInfo.Multiple * hp.Max * percent * 0.01d);
                long healingValue = DamageFormula.CalHealingValue(skillOwner.Entity.AttributeSet, addValue);
                skillOwner.Entity.AttributeSet.AddHP(excutor.SkillInfo.Owner.Entity, healingValue);
                //TODO 回血字体
            }
            else
            {
                CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(excutor.CombatType);

                IEntity[] list;
                if (excutor.SkillInfo.TargetIsPlayer)
                {
                    list = combatManager.Players;
                }
                else
                {
                    list = combatManager.Enemies;
                }

                for (int i = 0; i < list.Length; i++)
                {
                    var hp = list[i].AttributeSet.HPAttribute;
                    long addValue = CombatUtility.CeilToLong(fixedValue + excutor.SkillInfo.Multiple * hp.Max * percent * 0.01d);
                    long healingValue = DamageFormula.CalHealingValue(list[i].AttributeSet, addValue);
                    list[i].AttributeSet.AddHP(excutor.SkillInfo.Owner.Entity, healingValue);
                    //TODO 回血字体
                }
            }
        }
    }
}