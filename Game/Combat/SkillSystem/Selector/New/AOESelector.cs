using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Selector(ESelectType.AOE)]
    public class AOESelector : BaseSelector
    {
        List<IEntity> _cacheList = new List<IEntity>();

        public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor)
        {
            AreaDebug.DrawWireArea(skillExcutor.Position, Quaternion.identity, info.Config.DamageArea, 3, Color.red);
        }

        public override void Select(SkillInfo info, ISkillExcutor skillExcutor, HashSet<EntityInfo> infos)
        {
            _cacheList.Clear();
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(skillExcutor.CombatType);
            IEntity[] list;
            if (info.TargetIsPlayer)
            {
                list = combatManager.Players;
            }
            else
            {
                list = combatManager.Enemies;
            }

            CombatUtility.GetInAreaEntity(skillExcutor.Position, info.Config.DamageArea, _cacheList, list);

            foreach (var item in _cacheList)
            {
                infos.Add(EntityInfo.Build(item));
            }
        }
    }
}