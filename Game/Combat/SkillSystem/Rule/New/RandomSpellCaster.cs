using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Skill;
using UnityEngine;

namespace Combat
{
    [SkillRule(ESkillRuleType.RandomSpellCaster)]
    public class RandomSpellCaster : SkillExcutor
    {
        public override ISkillExcutor New() => new RandomSpellCaster();
        SkillInfo _skillInfo;
        int _num;
        float _timer;

        protected override void OnStart()
        {
            _num = 0;
            _timer = 0;
            int skillID = (int)SkillInfo.Config.RuleValues[1];//技能ID
            _skillInfo = this.GetSystem<ISkillFactory>().SpawnSkillInfo(SkillInfo.Owner, skillID);
            _skillInfo.SkillLevel = SkillInfo.SkillLevel;
        }

        protected override void OnUpdate(float deltaTime)
        {
            int num = (int)SkillInfo.Config.RuleValues[0];//数量
            float interval = SkillInfo.Config.RuleValues[2];//施法间隔

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(CombatType);

            if (_num < num)
            {
                if (_timer <= 0)
                {
                    SelectTargets();
                    EntityInfo[] list = Targets.ToArray();
                    if (list.Length > 0)
                    {
                        int randomIndex = combatManager.Random.GetRandom(0, list.Length);
                        Debug.Log($"Combat 随机索引为：{randomIndex}-{list.Length}");
                        ReleaseSkillSafe(_skillInfo, list[randomIndex]);
                    }

                    _num++;
                    _timer += interval;
                }
                _timer -= deltaTime;
            }
            else
            {
                Dispose();
                this.GetSystem<ISkillFactory>().RecycleSkillInfo(_skillInfo);
            }
        }

        void ReleaseSkillSafe(SkillInfo skillInfo, EntityInfo lockTarget)
        {
            if (skillInfo == null) return;
            if (skillInfo.Config == null) return;

            skillInfo.Owner.Entity.SkillOwner.CreateSkill(skillInfo, lockTarget);
        }
    }
}