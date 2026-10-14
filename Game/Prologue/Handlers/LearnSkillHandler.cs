using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Skill;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.LearnSkill)]
    public class LearnSkillHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new LearnSkillHandler();
        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();
        CombatManager _combatManager => _combatSystem.GetCombatManager(ECombatType.Prologue);

        protected override void OnStart()
        {
            PrologueSelectSkillPanel.OpenInfo openInfo = new PrologueSelectSkillPanel.OpenInfo
            {
                SkillList = Info.LearnSkillIDs,
                ClickAction = OnSelectSkill,
            };
            GameEntry.UI.Push<PrologueSelectSkillPanel, PrologueSelectSkillPanel.OpenInfo>(openInfo);
        }

        private void OnSelectSkill(int skillId)
        {
            SkillSystem skillSystem = _combatManager.TeamManager.Hero.Blackboard.GetGlobalReferenceValue<SkillSystem>(EntityBlackboardDefine.SKILL_SYSTEM);
            skillSystem.AddSkill(skillId, 1);
            Finish = true;
        }
    }
}