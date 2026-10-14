using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Skill;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Doppelganger)]
    public class DoppelgangerBuilder : EntityBuilder<DTPlayer>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Death, OnDeath);
            entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);
            entity.EventSource.Register<IEntity>(EntityEvent.Update, CheckDeath);
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return GameEntry.Resource.GetGlobalCacheAsset<ExternalBehavior>("CommonAI");
        }

        protected override string GetModelPath(DTPlayer data)
        {
            return $"prefab/spine/character/hero/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTPlayer data, bool isPlayer, BuildInfo buildInfo)
        {
            ISkillFactory skillFactory = this.GetSystem<ISkillFactory>();
            skillFactory.BindSkill(entity, new int[1] { data.General_skill });

            entity.IsMonster = false;
            entity.AnimationController.SetScale(data.Scale);
            entity.Carrer = (int)data.Career;

            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SUMMONER, buildInfo.Summoner);
            entity.Blackboard.SetGlobalFloatValue(EntityBlackboardDefine.SUMMON_DURATION, buildInfo.SummonDuration);

            DecorationManager decorationManager = new DecorationManager();
            decorationManager.Init(entity, false);
            SetDecorations(decorationManager, buildInfo);

            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.DECORATION_MANAGER, decorationManager);
        }

        private void OnRecycle(IEntity entity)
        {
            entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER).Clear();
        }

        private void OnDeath(IEntity entity)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);

            combatManager.RemoveOther(entity);
        }

        void SetDecorations(DecorationManager decorationManager, BuildInfo buildInfo)
        {
            decorationManager.SetEquip(buildInfo.PrimaryWeaponID);
            decorationManager.SetEquip(buildInfo.SecondaryWeaponID);
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