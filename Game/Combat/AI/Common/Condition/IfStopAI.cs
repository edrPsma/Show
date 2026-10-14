using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否停止AI
    /// </summary>
    public class IfStopAI : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (IAIAgent.StopAI)
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