using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 能否执行行为
    /// </summary>
    public class IfCanDoAction : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.AIAgent.CantDoAction == 0)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}