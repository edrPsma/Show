using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Summon)]
    public class SummonBuilder : EntityBuilder<DTMonster>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Update, CheckDeath);
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return GameEntry.Resource.GetGlobalCacheAsset<ExternalBehavior>("CommonAI");
        }

        protected override string GetModelPath(DTMonster data)
        {
            return $"prefab/spine/character/monster/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTMonster data, bool isPlayer, BuildInfo buildInfo)
        {
            ISkillFactory skillFactory = entity.GetSystem<ISkillFactory>();
            skillFactory.BindSkill(entity, new int[1] { data.General_skill });

            entity.IsMonster = isPlayer;
            entity.AnimationController.SetScale(data.Scale);
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SUMMONER, buildInfo.Summoner);
            entity.Blackboard.SetGlobalFloatValue(EntityBlackboardDefine.SUMMON_DURATION, buildInfo.SummonDuration);

            //设置属性
            entity.AttributeSet.SetValue(EAttributeKey.Health, 100);// 召唤物无法被选中，所以血量给100就行
        }

        private void CheckDeath(IEntity e)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(e.CombatType);
            float timer = e.Blackboard.GetGlobalFloatValue(EntityBlackboardDefine.SUMMON_DURATION);

            if (timer <= 0)
            {
                e.AttributeSet.HPAttribute.Value = 0;
                IEntity summoner = e.Blackboard.GetGlobalReferenceValue<IEntity>(EntityBlackboardDefine.SUMMONER);

                if (summoner == null)
                {
                    Debug.LogError("Combat 召唤者为空"); //若有bug则考虑使用entityinfo
                }
                else
                {
                    summoner.SummonManager.RemoveSummon(e);
                }
            }
            else
            {
                e.Blackboard.SetGlobalFloatValue(EntityBlackboardDefine.SUMMON_DURATION, timer - combatManager.GetDeltaTime());
            }

        }
    }
}