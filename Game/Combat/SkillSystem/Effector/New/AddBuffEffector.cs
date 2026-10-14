using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.AddBuff)]
    public class AddBuffEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            if (damageInfo.IsDodge) return;// 若闪避则不添加buff

            AddBuff(excutor, skillOwner, target, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            AddBuff(excutor, skillOwner, target, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            AddBuff(excutor, skillOwner, excutor.LockTarget, param);
        }

        void AddBuff(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, List<int> param)
        {
            int targetType = param[0];
            int buffID = param[1];
            int probability = param[2];

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(excutor.CombatType);

            int randomValue = combatManager.Random.GetRandom(0, 100);

            if (randomValue <= probability)
            {
                if (targetType == 0)// 仅施法者自身
                {
                    skillOwner.Entity.BuffOwner.AddBuff(buffID, skillOwner, excutor.SkillInfo.Multiple, excutor.SkillInfo.Data.ID, excutor.SkillInfo.SkillLevel);
                }
                else if (targetType == 1)// 技能伤害目标
                {
                    target.Entity.BuffOwner.AddBuff(buffID, skillOwner, excutor.SkillInfo.Multiple, excutor.SkillInfo.Data.ID, excutor.SkillInfo.SkillLevel);
                }
                else if (targetType == 3)// 释放者全体及其宠物
                {
                    //TODO 释放者全体及其宠物
                    skillOwner.Entity.BuffOwner.AddBuff(buffID, skillOwner, excutor.SkillInfo.Multiple, excutor.SkillInfo.Data.ID, excutor.SkillInfo.SkillLevel);
                }
            }
        }
    }
}