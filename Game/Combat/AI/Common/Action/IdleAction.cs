using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 闲置行为
    /// </summary>
    public class IdleAction : BaseAction
    {
        public override void OnStart()
        {
            _entity.AnimationController.PlayAnimation(EAnimationName.idle);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }
    }
}