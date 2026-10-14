using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Level;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.Boss)]
    public class BossBuilder : EntityBuilder<DTMonster>
    {
        protected override void BindEvent(IEntity entity)
        {
            entity.EventSource.Register<IEntity>(EntityEvent.Death, OnDeath);
            entity.AttributeSet.GetAttribute(EAttributeKey.MovementSpeed).Subscribe(value => OnMovementSpeedChange(entity, value));
            entity.AttributeSet.GetAttribute(EAttributeKey.MovementSpeedBonus).Subscribe(value => OnMovementSpeedBonusChange(entity, value));
        }

        private void OnMovementSpeedBonusChange(IEntity entity, long value)
        {
            long basic = entity.AttributeSet.GetValue(EAttributeKey.MovementSpeed);
            long speed = DamageFormula.CalMoveSpeed(basic, value);
            entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.MOVE_SPEED, (int)speed);
        }

        private void OnMovementSpeedChange(IEntity entity, long value)
        {
            long bonus = entity.AttributeSet.GetValue(EAttributeKey.MovementSpeedBonus);
            long speed = DamageFormula.CalMoveSpeed(value, bonus);
            entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.MOVE_SPEED, (int)speed);
        }

        private void OnDeath(IEntity entity)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);

            if (entity.CombatType == ECombatType.Mainline)
            {
                GameEntry.UI.SearchForm<DropItemPanel>()?.SpawnCoin(entity.Position);
                // GameEntry.UI.SearchForm<DropItemPanel>()?.SpawnExp(entity.Position);
            }

            GameEntry.Prefab.Instantiate("prefab/SkillPrefab/currency_death", prefab => DeathLoadOver(prefab, entity.AnimationController.BodyTrans.position, combatManager.GameSpeed.Value), null, EPrefabTag.Residency);
            combatManager.RemoveEnemy(entity);
        }

        private void DeathLoadOver(GameObject prefab, Vector3 pos, float speed)
        {
            prefab.transform.position = pos;
            ParticleInformation particleInformation = prefab.GetOrAddComponent<ParticleInformation>();
            particleInformation.Play(speed);
            GameEntry.Task.AddTask(_ =>
            {
                GameEntry.Prefab.Release(prefab, false);
            }).Delay(TimeSpan.FromSeconds(particleInformation.Duration)).Run();
        }

        protected override ExternalBehavior GetAIBehaviour()
        {
            return GameEntry.Resource.GetGlobalCacheAsset<ExternalBehavior>("EnemyAI");
        }

        protected override string GetModelPath(DTMonster data)
        {
            return $"prefab/spine/character/monster/{data.Model}";
        }

        protected override void SetParameter(Entity entity, DTMonster data, bool isPlayer, BuildInfo userData)
        {
            ISkillFactory skillFactory = entity.GetSystem<ISkillFactory>();

            List<int> skillList = new List<int> { data.General_skill };
            if (data.Technical_skill != 0)
            {
                skillList.Add(data.Technical_skill);
            }
            skillFactory.BindSkill(entity, skillList.ToArray());

            entity.IsMonster = true;
            entity.AnimationController.SetScale(data.Scale);
            entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.MONSTER_INDEX, userData.MonsterIndex);

            HPBar.SetHPBar("prefab/Combat/BossHpBar", -1, EntityInfo.Build(entity));
        }
    }
}