using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Skill;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Hero)]
    public class HeroBuilder : EntityBuilder<DTPlayer>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Death, OnDeath);
            entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);
            IUnRegister unRegister = GameEntry.Event.Register<EventAttributeChange>(e => OnAttributeChange(entity, e));
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.EQUIP_ATTRIBUTE_CHANGE_EVENT_UNREGISTER, unRegister);

            entity.AttributeSet.HPAttribute.Subscribe(value => OnHPChange(entity, value));
        }

        private void OnRecycle(IEntity entity)
        {
            entity.Blackboard.GetGlobalReferenceValue<IUnRegister>(EntityBlackboardDefine.EQUIP_ATTRIBUTE_CHANGE_EVENT_UNREGISTER).UnRegister();
            this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType).TeamManager.SetHeroEmpty();

            entity.Blackboard.GetGlobalReferenceValue<ISkillSystem>(EntityBlackboardDefine.SKILL_SYSTEM).Clear();
            entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER).Clear();
        }

        private void OnAttributeChange(IEntity entity, EventAttributeChange e)
        {
            if (e.ChangeDic != null)
            {
                foreach (var item in e.ChangeDic)
                {
                    entity.AttributeSet.AddValue(item.Key, item.Value);
                }
            }
        }

        private void OnHPChange(IEntity entity, long value)
        {
            GameEntry.Event.Trigger(new EventHeroHpChange { CurrentHP = value, MaxHP = entity.AttributeSet.HPAttribute.Max });
        }

        private void OnDeath(IEntity entity)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);

            combatManager.TeamManager.SetHeroEmpty();
            combatManager.RemovePlayer(entity);
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
            entity.AnimationController.SetScale(data.Scale);
            entity.Carrer = (int)data.Career;

            SkillSystem skillSystem = new SkillSystem();
            skillSystem.RefreshSkillList = userData.RefreshSkillList;
            skillSystem.ProcessingSkillLevelEvent = true;

            DecorationManager decorationManager = new DecorationManager();
            decorationManager.Init(entity, true);
            SetDecorations(decorationManager);

            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SKILL_SYSTEM, skillSystem);
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.DECORATION_MANAGER, decorationManager);

            HPBar.SetHPBar("prefab/Combat/HeroHpBar", 1, EntityInfo.Build(entity));
        }

        void SetDecorations(DecorationManager decorationManager)
        {
            //TODO 待添加
            IEquipSytem equipSytem = this.GetSystem<IEquipSytem>();

            decorationManager.SetEquip(equipSytem.GetEquipID(DTEquip.EType.Zhu_wu_qi));
            decorationManager.SetEquip(equipSytem.GetEquipID(DTEquip.EType.Fu_wu_qi));
            decorationManager.SetMount(this.GetModel<IMountModel>().UseMountID.Value);
            decorationManager.SetWing(this.GetModel<IWingModel>().UseWingID.Value);
        }
    }
}