using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.ExtraAttack)]
    public class ExtraAttackEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            ExtraAttack(excutor, target, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            ExtraAttack(excutor, target, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            ExtraAttack(excutor, excutor.LockTarget, param);
        }

        void ExtraAttack(SkillExcutor excutor, EntityInfo target, List<int> param)
        {
            int probability = param[0];
            int minNum = param[1];
            int maxNum = param[2];

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(excutor.CombatType);

            int randomValue = combatManager.Random.GetRandom(0, 100);
            if (randomValue <= probability)
            {
                int num = combatManager.Random.GetRandom(minNum, maxNum);
                for (int i = 0; i < num; i++)
                {
                    excutor.SkillInfo.Owner.Entity.SkillOwner.CreateSkill(excutor.SkillInfo, target);
                }
            }
        }
    }
}