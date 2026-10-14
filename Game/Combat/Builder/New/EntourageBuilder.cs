using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Entourage)]
    public class EntourageBuilder : EntityBuilder<DTPet_advanced>
    {
        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();

        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);
            IUnRegister unRegister = GameEntry.Event.Register<EventPetAttributeChange>(e => OnAttributeChange(entity, e));
            entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.EQUIP_ATTRIBUTE_CHANGE_EVENT_UNREGISTER, unRegister);
        }

        private void OnAttributeChange(IEntity entity, EventPetAttributeChange e)
        {
            if (entity.EntityID != e.PetID) return;

            if (e.ChangeDic != null)
            {
                foreach (var item in e.ChangeDic)
                {
                    // 过滤宠物属性
                    if (item.Key == (int)EAttributeKey.PetAttackPowerBonus || item.Key == (int)EAttributeKey.PetAttackSpeedBonus)
                    {
                        IEntity hero = _combatSystem.GetCombatManager(entity.CombatType).TeamManager.Hero;

                        if (hero != null)
                        {
                            hero.AttributeSet.AddValue(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        entity.AttributeSet.AddValue(item.Key, item.Value);
                    }

                }
            }
        }

        private void OnRecycle(IEntity e)
        {
            e.Blackboard.GetGlobalReferenceValue<IUnRegister>(EntityBlackboardDefine.EQUIP_ATTRIBUTE_CHANGE_EVENT_UNREGISTER)?.UnRegister();
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return GameEntry.Resource.GetGlobalCacheAsset<ExternalBehavior>("CommonAI");
        }

        protected override string GetModelPath(DTPet_advanced data)
        {
            return $"prefab/spine/character/monster/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTPet_advanced data, bool isPlayer, BuildInfo buildInfo)
        {
            ISkillFactory skillFactory = entity.GetSystem<ISkillFactory>();
            skillFactory.BindSkill(entity, new int[1] { data.Attack });

            entity.AnimationController.SetScale(data.zoom);

            entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.PET_INDEX, buildInfo.PetIndex);

            entity.IsMonster = false;
        }
    }
}