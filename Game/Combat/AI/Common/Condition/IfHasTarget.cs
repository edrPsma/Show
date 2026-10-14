using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否有目标
    /// </summary>
    public class IfHasTarget : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.AIAgent.Target.IsValid)
            {
                if (_entity.AIAgent.Target.IsAlive)
                {
                    return TaskStatus.Success;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}