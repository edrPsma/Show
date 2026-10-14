using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Combat
{
    [SkillRule(ESkillRuleType.RotateProjectBullets)]
    public class AdventurerAttack : SkillExcutor//219001
    {
        public override ISkillExcutor New() => new AdventurerAttack();
        Vector3 _controlPos;
        float _time;
        Vector3 _startPos;
        Vector3 _endPos;
        float _dis;
        SpriteRenderer _sr;

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

        protected override void OnPrefabLoadOver()
        {
            _sr = _prefab.GetComponentInChildren<SpriteRenderer>();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            _sr = null;
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_sr != null)
            {
                _sr.transform.eulerAngles -= Vector3.forward * SkillInfo.Config.RuleValues[2];
            }
            float speed = SkillInfo.Config.RuleValues[0];
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(CombatType);
            var step = combatManager.GetDeltaTime() * speed / _dis;
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