using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Doppelganger)]
    public class DoppelgangerEffector : BaseEffector
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
            int ratio = param[0];// 单位继承攻击属性比例
            int duration = param[1];// 持续时间
            int skillLv = excutor.SkillInfo.SkillLevel;

            DecorationManager decorationManager = skillOwner.Entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER);

            // 分身外观只读取玩家时装、武器和副武器
            BuildInfo buildInfo = new BuildInfo
            {
                SummonDuration = duration * 0.001f,
                Summoner = excutor.SkillInfo.Owner.Entity,
                PrimaryWeaponID = decorationManager.GetPartIDByDecorationType(EDecorationType.PrimaryWeapon),
                SecondaryWeaponID = decorationManager.GetPartIDByDecorationType(EDecorationType.SecondaryWeapon),
            };

            IEntity entity = this.GetSystem<IEntityFactory>().Create(excutor.CombatType, EEntityType.Doppelganger, skillOwner.Entity.EntityID, skillOwner.Entity.IsPlayer, buildInfo);

            float FinalMagnification = ratio + skillLv * excutor.SkillInfo.Data.Multiple * ratio;
            foreach (EAttributeKey key in Enum.GetValues(typeof(EAttributeKey)))
            {
                long value = CombatUtility.CeilToLong(skillOwner.Entity.AttributeSet.GetValue(key) * FinalMagnification * 0.0001d);
                entity.AttributeSet.SetValue(key, value);
            }

            entity.AttributeSet.ResetHP(100, 0, 100);// 召唤物无法被选中，所以血量给100就行
            skillOwner.Entity.SummonManager.AddSummon(entity);
            entity.AIAgent.StartBehavior();
        }
    }
}