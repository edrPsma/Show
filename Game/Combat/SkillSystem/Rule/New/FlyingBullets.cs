using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.FlyingBullets)]
    public class FlyingBullets : SkillExcutor
    {
        public override ISkillExcutor New() => new FlyingBullets();
        Vector3 _dir;

        protected override void OnStart()
        {
            SelectTargets();
            // ImpactTargetsBeforeDamage();
            // DisplayAllTrail();
            Vector3 targetPos = _lockTargetBodyPos;
            Vector3 self = Position;
            _dir = (targetPos - self).normalized;
            SetRotation(Quaternion.FromToRotation(Vector3.right, _dir * Direction));
        }

        protected override void OnUpdate(float deltaTime)
        {
            float speed = SkillInfo.Config.RuleValues[0];
            float step = deltaTime * speed;
            float m = _dir.x > 0 ? 1 : -1;

            if ((Position.x - _lockTargetBodyPos.x) * m > 0)
            {
                StartDamage();
            }
            else
            {
                SetPosition(Position + step * _dir);
            }
        }
    }
}