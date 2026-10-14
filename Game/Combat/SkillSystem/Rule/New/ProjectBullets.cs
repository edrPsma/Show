using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.ProjectBullets)]
    public class ProjectBullets : SkillExcutor
    {
        public override ISkillExcutor New() => new ProjectBullets();
        Vector3 _controlPos;
        float _time;
        Vector3 _startPos;
        Vector3 _endPos;
        float _dis;

        protected override void OnStart()
        {
            // DisplayAllTrail();

            _time = 0;
            float angle = SkillInfo.Config.RuleValues[1] * Mathf.Deg2Rad;
            _startPos = Position;
            _endPos = _lockTargetBodyPos;
            Vector3 mid = Vector3.Lerp(Position, _lockTargetBodyPos, 0.5f);
            _dis = Vector3.Distance(Position, _lockTargetBodyPos);
            _controlPos = mid + new Vector3(0, Mathf.Tan(angle) * _dis * 0.5f, 0);
        }

        protected override void OnUpdate(float deltaTime)
        {
            float speed = SkillInfo.Config.RuleValues[0];
            var step = deltaTime * speed / _dis;
            _time = Mathf.Clamp01(_time + step);

            if (Mathf.Abs(Position.x - _endPos.x) <= 0.1f)
            {
                StartDamage();
            }
            else
            {
                Vector3 pos = BezierTools.SecondRrder(_time, _startPos, _controlPos, _endPos);
                SetRotation(Quaternion.FromToRotation(Vector3.right, (pos - Position) * Direction));
                SetPosition(pos);
            }
        }
    }
}