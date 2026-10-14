using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI.Enemy
{
    public class MoveAction : BaseAction
    {
        public override void OnStart()
        {
            _entity.AnimationController.PlayAnimation(EAnimationName.run);
        }

        public override TaskStatus OnUpdate()
        {
            EntityInfo targetInfo = _entity.AIAgent.Target;

            if (!targetInfo.IsValid || !targetInfo.IsAlive)
            {
                return TaskStatus.Failure;
            }

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(_entity.CombatType);

            int speed = _entity.Blackboard.GetGlobalIntValue(EntityBlackboardDefine.MOVE_SPEED);
            _entity.GameObject.transform.position += _entity.Direction * combatManager.GetDeltaTime() * Vector3.right * speed * 0.0001f;
            return TaskStatus.Running;
        }
    }
}