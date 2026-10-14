using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Divine)]
    public class DivineCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var divineModel = this.GetModel<IDivineModel>();
            var atts = divineModel.CalculateAttribute(divineModel.TotalLevel);
            foreach (var att in atts)
            {
                list.Add(new PropertyInfo(att.Key, att.Value));
            }

            //当前是否 装备了
            if (divineModel.UseFightDivineID.Value != 0)
            {
                var data = divineModel.UnLocks[divineModel.UseFightDivineID.Value];
                atts = divineModel.CalculateAttributeStar(data.divineId, data.star);
                foreach (var att in atts)
                {
                    list.Add(new PropertyInfo(att.Key, att.Value));
                }
            }
        }

        public override void CalculatingLocalSkill(List<SkillLevelInfo> skillList)
        {
            var divineModel = this.GetModel<IDivineModel>();
            var cfg = divineModel.GetDivineCfg(divineModel.UseFightDivineID.Value);
            if (cfg == null || cfg.Skill == 0) return;

            skillList.Add(new SkillLevelInfo(cfg.Skill, 1));
        }

        public override void CalculatingNetSkill(List<SkillLevelInfo> skillList, MSG_Detail data)
        {
            if (data.m_skillDivine == 0)
                return;
            var divineModel = this.GetModel<IDivineModel>();
            var cfg = divineModel.GetDivineCfg(data.m_skillDivine);
            skillList.Add(new SkillLevelInfo(cfg.Skill, 1));
        }
    }
}