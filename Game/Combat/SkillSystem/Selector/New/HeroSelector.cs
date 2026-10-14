using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    [Selector(ESelectType.Hero)]
    public class HeroSelector : BaseSelector
    {
        public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor) { }

        public override void Select(SkillInfo info, ISkillExcutor skillExcutor, HashSet<EntityInfo> infos)
        {
            IEntity entity;
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(info.Owner.Entity.CombatType);

            if (info.TargetIsPlayer)
            {
                entity = combatManager.Players.Where(entity => entity.EntityType == EEntityType.Hero).FirstOrDefault();
            }
            else
            {
                entity = combatManager.Enemies.Where(entity => entity.EntityType == EEntityType.Hero).FirstOrDefault();
            }
            if (entity != null)
            {
                infos.Add(EntityInfo.Build(entity));
            }
        }
    }
}