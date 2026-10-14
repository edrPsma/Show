using System;
using System.Collections;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.Spellcaster)]
    public class Spellcaster : SkillExcutor
    {
        public override ISkillExcutor New() => new Spellcaster();

        SkillInfo _skillInfo;
        int _num;
        float _timer;

        protected override void OnStart()
        {
            _num = 0;
            _timer = 0;
            int skillID = (int)SkillInfo.Config.RuleValues[1];//技能ID
            _skillInfo = this.GetSystem<ISkillFactory>().SpawnSkillInfo(SkillInfo.Owner, skillID);
            _skillInfo.SkillLevel = SkillInfo.SkillLevel;// 技能等级使用当前技能的等级
        }

        protected override void OnUpdate(float deltaTime)
        {
            int num = (int)SkillInfo.Config.RuleValues[0];//数量
            float interval = SkillInfo.Config.RuleValues[2];//施法间隔

            if (_num < num)
            {
                if (_timer <= 0)
                {
                    ReleaseSkillSafe(_skillInfo, LockTarget);

                    _num++;
                    _timer += interval;
                }
                _timer -= deltaTime;
            }
            else
            {
                Dispose();
            }
        }

        protected override void OnDispose()
        {
            this.GetSystem<ISkillFactory>().RecycleSkillInfo(_skillInfo);
        }

        void ReleaseSkillSafe(SkillInfo skillInfo, EntityInfo lockTarget)
        {
            if (skillInfo == null) return;
            if (skillInfo.Config == null) return;

            skillInfo.Owner.Entity.SkillOwner.CreateSkill(skillInfo, lockTarget);
        }
    }
}