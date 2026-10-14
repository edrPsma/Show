using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Halidom)]
    public class HalidomCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var halidomModel = this.GetModel<IHalidomModel>();
            var atts = halidomModel.GetAllAttribute();
            foreach (var att in atts)
            {
                list.Add(new PropertyInfo(att.Key, att.Value));
            }
        }

        public override void CalculatingLocalSkill(List<SkillLevelInfo> skillList)
        {
            var halidomModel = this.GetModel<IHalidomModel>();
            if (halidomModel.Suit3_Id.Value != 0)
            {
                var cfg = halidomModel.GetSacredGroup(halidomModel.Suit3_Id.Value);
                skillList.Add(new SkillLevelInfo(cfg.piece_language3, 1));

                if (halidomModel.Suit6_Id.Value != 0)
                {
                    skillList.Add(new SkillLevelInfo(cfg.piece_language6, 1));
                }

            }
        }

        public override void CalculatingNetSkill(List<SkillLevelInfo> skillList, MSG_Detail data)
        {
            var halidomModel = this.GetModel<IHalidomModel>();
            var listAarrays = data.m_carrySacred;
            listAarrays.Sort();
            int count = 1;
            int suitID = 0;
            for (int i = 1; i < listAarrays.Count; i++)
            {
                if (listAarrays[i] == listAarrays[i - 1])
                {
                    count++;
                    if (count >= 3)
                        suitID = listAarrays[i];
                }
            }
            if (suitID != 0)
            {
                var cfg = halidomModel.GetSacredGroup(suitID);
                skillList.Add(new SkillLevelInfo(cfg.piece_language3, 1));
                if (count >= 6)
                {
                    skillList.Add(new SkillLevelInfo(cfg.piece_language6, 1));
                }
            }
        }
    }
}