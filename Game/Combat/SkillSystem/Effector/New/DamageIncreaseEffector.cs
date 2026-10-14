using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTSkill_buff;

namespace Combat
{
    [Effector(EEffectType.DamageIncrease)]
    public class DamageIncreaseEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            DamageIncrease(target, ref damageInfo, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Debug.LogError("Combat 伤害提高效果触发时机不正确");
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Debug.LogError("Combat 伤害提高效果触发时机不正确");
        }

        void DamageIncrease(EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            int increaseStep = param[0];
            ETage tage1 = (ETage)param[1];
            ETage tage2 = (ETage)param[2];
            ETage tage3 = (ETage)param[3];

            int increaseValue = 100;
            int[] buffs = target.Entity.BuffOwner.GetAllBuff();
            for (int i = 0; i < buffs.Length; i++)
            {
                var data = DataTable.GetItem<DTSkill_buff>(buffs[i]);
                if (data.Tage == tage1 || data.Tage == tage2 || data.Tage == tage3)
                {
                    increaseValue += increaseStep;
                }
            }

            damageInfo.FinalDamageValue = CombatUtility.CeilToLong(damageInfo.FinalDamageValue * (1 + increaseValue * 0.01d));
        }
    }
}