using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Skill;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Pvp)]
    public class PvpBuilder : EntityBuilder<DTPlayer>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Death, OnDeath);
            entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);
        }

        private void OnRecycle(IEntity entity)
        {
            if (entity.IsPlayer)
            {
                this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType).TeamManager.SetHeroEmpty();
            }

            entity.Blackboard.GetGlobalReferenceValue<ISkillSystem>(EntityBlackboardDefine.SKILL_SYSTEM).Clear();
            entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER).Clear();
        }

        private void OnDeath(IEntity entity)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);

            if (entity.IsPlayer)
            {
                combatManager.TeamManager.SetHeroEmpty();
                combatManager.RemovePlayer(entity);
            }
            else
            {
                combatManager.RemoveEnemy(entity);
            }
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return GameEntry.Resource.GetGlobalCacheAsset<ExternalBehavior>("CommonAI");
        }

        protected override string GetModelPath(DTPlayer data)
        {
            return $"prefab/spine/character/hero/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTPlayer data, bool isPlayer, BuildInfo userData)
        {
            ISkillFactory skillFactory = this.GetSystem<ISkillFactory>();
            skillFactory.BindSkill(entity, new int[1] { data.General_skill });

            entity.IsMonster = false;
            entity.EntityType = EEntityType.Hero;
            entity.Carrer = (int)data.Career;
            entity.AnimationController.SetScale(data.Scale);
            entity.SetDirection(isPlayer ? 1 : -1);

            SkillSystem skillSystem = new SkillSystem();
            skillSystem.RefreshSkillList = userData.RefreshSkillList;
            skillSystem.ProcessingSkillLevelEvent = false;

            DecorationManager decorationManager = new DecorationManager();
            decorationManager.Init(entity, false);

            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SKILL_SYSTEM, skillSystem);
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.DECORATION_MANAGER, decorationManager);

            HPBar.SetHPBar("prefab/Combat/HeroHpBar", entity.Direction, EntityInfo.Build(entity));
        }
    }
}