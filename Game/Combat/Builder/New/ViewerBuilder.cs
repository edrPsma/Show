using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Skill;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Viewer)]
    public class ViewerBuilder : EntityBuilder<DTPlayer>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);
        }

        private void OnRecycle(IEntity entity)
        {
            entity.Blackboard.GetGlobalReferenceValue<ISkillSystem>(EntityBlackboardDefine.SKILL_SYSTEM).Clear();
            entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER).Clear();
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return null;
        }

        protected override string GetModelPath(DTPlayer data)
        {
            return $"prefab/spine/character/hero/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTPlayer data, bool isPlayer, BuildInfo userData)
        {
            entity.IsMonster = false;
            entity.EntityType = EEntityType.Hero;
            entity.SetDirection(1);
            entity.Carrer = (int)data.Career;
            entity.AnimationController.SetScale(1);

            SkillSystem skillSystem = new SkillSystem();

            DecorationManager decorationManager = new DecorationManager();
            decorationManager.Init(entity, false);

            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SKILL_SYSTEM, skillSystem);
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.DECORATION_MANAGER, decorationManager);
        }
    }
}