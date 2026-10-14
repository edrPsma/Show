using System.Collections;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Summon)]
    public class SummonEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Summon(excutor, skillOwner, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Summon(excutor, skillOwner, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Summon(excutor, skillOwner, param);
        }

        void Summon(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            int num = param[0];// 召唤数量
            int monsterID = param[1];// 召唤单位ID
            int ratio = param[2];// 单位继承攻击属性比例
            int duration = param[3];// 持续时间

            BuildInfo buildInfo = new BuildInfo
            {
                SummonDuration = duration * 0.001f,
                Summoner = excutor.SkillInfo.Owner.Entity,
            };

            for (int i = 0; i < num; i++)
            {
                IEntity entity = this.GetSystem<IEntityFactory>().Create(excutor.CombatType, EEntityType.Summon, monsterID, skillOwner.Entity.IsPlayer, buildInfo);

                float FinalMagnification = excutor.SkillInfo.Data.Multiple * ratio;
                long value = CombatUtility.CeilToLong(skillOwner.Entity.AttributeSet.GetValue(EAttributeKey.AttackPower) * FinalMagnification * 0.0001d);
                entity.AttributeSet.SetValue(EAttributeKey.AttackPower, value);

                entity.AttributeSet.ResetHP(100, 0, 100);// 召唤物无法被选中，所以血量给100就行
                skillOwner.Entity.SummonManager.AddSummon(entity);
                entity.AIAgent.StartBehavior();
            }
        }
    }
}