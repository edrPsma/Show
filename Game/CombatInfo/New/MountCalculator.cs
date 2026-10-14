using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Mount)]
    public class MountCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var mountModel = this.GetModel<IMountModel>();
            var atts = mountModel.CalculateAttribute(mountModel.TotalLevel);
            foreach (var att in atts)
            {
                list.Add(new PropertyInfo(att.Key, att.Value));
            }
            //当前是否 装备了
            if (mountModel.UseFightMountID.Value != 0)
            {
                var data = mountModel.UnLocks[mountModel.UseFightMountID.Value];
                atts = mountModel.CalculateAttributeStar(data.mountId, data.star);
                foreach (var att in atts)
                {
                    list.Add(new PropertyInfo(att.Key, att.Value));
                }
            }
        }

        public override void CalculatingLocalSkill(List<SkillLevelInfo> skillList)
        {
            var mountModel = this.GetModel<IMountModel>();
            var cfg = mountModel.GetMountCfg(mountModel.UseFightMountID.Value);
            if (cfg == null || cfg.Skill == 0) return;

            skillList.Add(new SkillLevelInfo(cfg.Skill, 1));
        }

        public override void CalculatingNetSkill(List<SkillLevelInfo> skillList, MSG_Detail data)
        {
            if (data.m_skillMount == 0) return;

            var mountModel = this.GetModel<IMountModel>();

            var cfg = mountModel.GetMountCfg(data.m_skillMount);
            skillList.Add(new SkillLevelInfo(cfg.Skill, 1));
        }

    }
}