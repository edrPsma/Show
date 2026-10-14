using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.GoAhead)]
    public class GoAhead : SkillExcutor
    {
        public override ISkillExcutor New() => new GoAhead();
        Vector3 _endPos;

        protected override void OnStart()
        {
            // DisplayAllTrail();
            _endPos = Position + Vector3.right * Direction * SkillInfo.Config.RuleValues[1];
            StartDamage();
        }

        protected override void OnUpdate(float deltaTime)
        {
            float speed = SkillInfo.Config.RuleValues[0];
            float step = deltaTime * speed * Direction;

            if (Position.x + step >= _endPos.x)
            {
                Dispose();
            }
            else
            {
                SetPosition(Position + step * Vector3.right);
            }
        }
    }
}