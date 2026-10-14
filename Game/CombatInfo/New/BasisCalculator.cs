using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using RolePet;
using Skill;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Basis)]
    public class BasisCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            IRoleModel roleModel = this.GetModel<IRoleModel>();
            var data = DataTable.GetItem<DTPlayer>(roleModel.PlayerID.Value);
            for (int i = 0; i < data.Attr.Count; i++)
            {
                int key = data.Attr[i][0];
                int value = data.Attr[i][1];

                PropertyInfo propertyInfo = new PropertyInfo(key, value);

                list.Add(propertyInfo);
            }
        }

        public override void CalculatingNetProperty(List<PropertyInfo> list, MSG_Detail data)
        {
            for (int i = 0; i < data.m_properties.Count; i++)
            {
                var cur = data.m_properties[i];

                list.Add(new PropertyInfo(cur));
            }
        }

        public override void CalculatingLocalPetProperty(int petID, List<PropertyInfo> list)
        {
            if (PetUtility.TryGetPetInfoNode(petID, this.GetModel<IPetModel>(), out var petInfo))
            {
                for (int i = 0; i < petInfo.propertyList.Count; i++)
                {
                    int attrID = petInfo.propertyList[i].ID;
                    long attrVal = petInfo.propertyList[i].Value;
                    PropertyInfo info = list.Find(d => d.ID == attrID);
                    if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                    else info.Value += attrVal;
                }
            }
        }

        public override void CalculatingNetPetProperty(List<PropertyInfo> list, MSG_DetailPet data)
        {
            for (int i = 0; i < data.m_properties.Count; i++)
            {
                var cur = data.m_properties[i];

                list.Add(new PropertyInfo(cur));
            }
        }

        public override void CalculatingLocalSkill(List<SkillLevelInfo> skillList)
        {
            ISkillModel skillModel = this.GetModel<ISkillModel>();
            IRoleModel roleModel = this.GetModel<IRoleModel>();
            var data = DataTable.GetItem<DTPlayer>(roleModel.PlayerID.Value);

            skillList.Add(new SkillLevelInfo(data.Technical_skill, 1));

            foreach (var skillID in skillModel.SkillList)
            {
                var info = skillModel.GetSkillDataFromSkillId(skillID);
                int skillLevel = info == null ? 1 : info.level;

                SkillLevelInfo levelInfo = new SkillLevelInfo(skillID, skillLevel);
                skillList.Add(levelInfo);
            }
        }

        public override void CalculatingNetSkill(List<SkillLevelInfo> skillList, MSG_Detail data)
        {
            IRoleModel roleModel = this.GetModel<IRoleModel>();

            int heroID = roleModel.CalPlayerID(data.m_model.m_sex, data.m_model.m_career, data.m_model.m_careerStage);

            var table = DataTable.GetItem<DTPlayer>(heroID);
            skillList.Add(new SkillLevelInfo(table.Technical_skill, 1));

            for (int i = 0; i < data.m_carrySkills.Count; i++)
            {
                skillList.Add(new SkillLevelInfo(data.m_carrySkills[i]));
            }
        }
    }
}