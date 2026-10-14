using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.GoAheadOnlyOne)]
    public class GoAheadOnlyOne : SkillExcutor
    {
        public override ISkillExcutor New() => new GoAheadOnlyOne();

        HashSet<EntityInfo> _entities = new HashSet<EntityInfo>();// 存储被攻击过的单位
        Vector3 _endPos;

        protected override void OnStart()
        {
            _entities.Clear();
            // DisplayAllTrail();
            _endPos = Position + Vector3.right * Direction * SkillInfo.Config.RuleValues[1];
            StartDamage();
        }

        public override void OnTakeDamage(EntityInfo target)
        {
            _entities.Add(target);
        }

        public override void SelectTargets()
        {
            base.SelectTargets();
            foreach (var item in _entities)
            {
                Targets.Remove(item);
            }
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