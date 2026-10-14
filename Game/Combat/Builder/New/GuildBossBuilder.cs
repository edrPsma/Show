using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Guild;
using Level;
using UnityEngine;

namespace Combat
{
    [Builder(EEntityType.GuildBoss)]
    public class GuildBossBuilder : EntityBuilder<DTMonster>
    {
        protected override void BindEvent(IEntity entity)
        {

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
        }
    }
}