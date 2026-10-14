using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.AI
{
    public class SelectTargetAction : BaseAction
    {
        public override void OnStart()
        {
            // _entity.AnimationController.PlayAnimation(EAnimationName.idle);
            CombatManager combatManager = _entity.GetSystem<ICombatSystem>().GetCombatManager(_entity.CombatType);
            if (_entity.IsPlayer)
            {
                IEntity target = CombatUtility.GetNearestEntity(_entity.GameObject.transform.position, combatManager.Enemies);
                _entity.AIAgent.SetTarget(target);
            }
            else
            {
                IEntity target = CombatUtility.GetNearestEntity(_entity.GameObject.transform.position, combatManager.Players);
                _entity.AIAgent.SetTarget(target);
            }
        }
    }
}