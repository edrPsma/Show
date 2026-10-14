using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否有下一个技能
    /// </summary>
    public class IfHasNextSkill : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.SkillOwner.NextSkill != null)
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