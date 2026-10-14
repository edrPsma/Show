using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否死亡
    /// </summary>
    public class IfDeadth : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.AttributeSet.HP <= 0)
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