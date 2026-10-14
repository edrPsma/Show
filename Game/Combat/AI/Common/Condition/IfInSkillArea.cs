using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 是否在攻击范围内
    /// </summary>
    public class IfInSkillArea : BaseCondition
    {
        public override TaskStatus OnUpdate()
        {
            if (_entity.AIAgent.Target.IsValid)
            {
                if (_entity.SkillOwner.NextSkill == null) return TaskStatus.Failure;

                float area = _entity.SkillOwner.NextSkill.Config.SelectArea;
                float dis = Mathf.Abs(_entity.AIAgent.Target.Position.x - _entity.Position.x);

                if (dis <= area)
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