using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否能够移动
    /// </summary>
    public class IfCanMove : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.AIAgent.CantMove == 0)
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