using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using RolePet;
using UnityEngine;

namespace Combat.CombatInformation
{
    public static class CombatInfoTool
    {
        public static CombatInfo CalculatingLocalInfo()
        {
            CombatInfo combatInfo = new CombatInfo();

            List<PropertyInfo> property = new List<PropertyInfo>();
            List<SkillLevelInfo> skillList = new List<SkillLevelInfo>();
            List<PetInfo> petList = new List<PetInfo>();

            Dictionary<object, object> dic = GameEntry.Reflection.GetDic<BaseCombatInfoCalculator>();

            foreach (var item in dic)
            {
                BaseCombatInfoCalculator calculator = item.Value as BaseCombatInfoCalculator;

                calculator.CalculatingLocalProperty(property);
                calculator.CalculatingLocalSkill(skillList);
            }

            IPetModel petModel = combatInfo.GetModel<IPetModel>();
            var carry = petModel.Carries.Find(d => d.isUsed);
            if (carry != null)
            {
                for (int i = 0; i < carry.petIDs.Count; i++)
                {
                    int petID = carry.petIDs[i];

                    if (PetUtility.TryGetPetInfoNode(petID, petModel, out var result))
                    {
                        PetInfo petInfo = new PetInfo();
                        petInfo.PetID = petID;
                        petInfo.Advanced = result.advance;
                        petInfo.Property = new List<PropertyInfo>();

                        foreach (var item in dic)
                        {
                            BaseCombatInfoCalculator calculator = item.Value as BaseCombatInfoCalculator;

                            calculator.CalculatingLocalPetProperty(petID, petInfo.Property);
                        }

                        FilterAndAddPetProperties(property, petInfo.Property);

                        petList.Add(petInfo);
                    }
                }
            }

            combatInfo.Property = property;
            combatInfo.SkillList = skillList;
            combatInfo.PetList = petList;
            combatInfo.WingID = combatInfo.GetModel<IWingModel>().UseWingID.Value;
            combatInfo.MountID = combatInfo.GetModel<IMountModel>().UseMountID.Value;
            IEquipModel equipModel = combatInfo.GetModel<IEquipModel>();
            var pSubID = equipModel.GetWears(DTEquip.EType.Zhu_wu_qi);
            if (pSubID != null)
            {
                combatInfo.PrimaryWeaponID = pSubID.id;
            }
            var sSubID = equipModel.GetWears(DTEquip.EType.Fu_wu_qi);
            if (sSubID != null)
            {
                combatInfo.SecondaryWeaponID = sSubID.id;
            }

            return combatInfo;
        }

        public static CombatInfo CalculatingNetInfo(MSG_Detail detail)
        {
            CombatInfo combatInfo = new CombatInfo();

            List<PropertyInfo> property = new List<PropertyInfo>();
            List<SkillLevelInfo> skillList = new List<SkillLevelInfo>();
            List<PetInfo> petList = new List<PetInfo>();

            Dictionary<object, object> dic = GameEntry.Reflection.GetDic<BaseCombatInfoCalculator>();

            foreach (var item in dic)
            {
                BaseCombatInfoCalculator calculator = item.Value as BaseCombatInfoCalculator;

                calculator.CalculatingNetProperty(property, detail);
                calculator.CalculatingNetSkill(skillList, detail);
            }

            var carry = detail.m_carryPets;
            if (carry != null)
            {
                for (int i = 0; i < carry.Count; i++)
                {

                    PetInfo petInfo = new PetInfo();
                    petInfo.PetID = carry[i].m_id;
                    petInfo.Advanced = carry[i].m_advanced;
                    petInfo.Property = new List<PropertyInfo>();

                    foreach (var item in dic)
                    {
                        BaseCombatInfoCalculator calculator = item.Value as BaseCombatInfoCalculator;

                        calculator.CalculatingNetPetProperty(petInfo.Property, carry[i]);
                    }

                    FilterAndAddPetProperties(property, petInfo.Property);

                    petList.Add(petInfo);
                }
            }

            combatInfo.Property = property;
            combatInfo.SkillList = skillList;
            combatInfo.PetList = petList;
            combatInfo.WingID = detail.m_model.m_showWing;
            combatInfo.MountID = detail.m_model.m_showMount;
            GetWeapons(detail.m_model, out combatInfo.PrimaryWeaponID, out combatInfo.SecondaryWeaponID);

            return combatInfo;
        }

        public static int GetPetAdvancedId(int petId, int advanced)
        {
            return 500000 + petId * 1000 + advanced;
        }

        public static void SetProperty(List<PropertyInfo> properties, IEntity entity)
        {
            foreach (var item in properties)
            {
                entity.AttributeSet.AddValue(item.ID, item.Value);
            }

            entity.AttributeSet.ResetHP(DamageFormula.CalMaxHP(entity.AttributeSet), 0, DamageFormula.CalMaxHP(entity.AttributeSet));
        }

        public static void GetWeapons(MSG_DetailModel detailModel, out int primaryWeapon, out int secondaryWeapon)
        {
            primaryWeapon = 0;
            secondaryWeapon = 0;

            for (int i = 0; i < detailModel.m_equips.Count; i++)
            {
                int id = detailModel.m_equips[i].m_equipId;
                var config = DataTable.GetItem<DTEquip>(id);

                if (config.Type == DTEquip.EType.Zhu_wu_qi)
                {
                    primaryWeapon = id;
                }
                else if (config.Type == DTEquip.EType.Fu_wu_qi)
                {
                    secondaryWeapon = id;
                }
            }
        }

        static void FilterAndAddPetProperties(List<PropertyInfo> roleProperty, List<PropertyInfo> petProperty)
        {
            int key1 = (int)EAttributeKey.PetAttackPowerBonus;
            int key2 = (int)EAttributeKey.PetAttackSpeedBonus;

            for (int i = petProperty.Count - 1; i >= 0; i--)
            {
                PropertyInfo cur = petProperty[i];

                if (cur.ID == key1 || cur.ID == key2)
                {
                    petProperty.Remove(cur);
                    roleProperty.Add(cur);
                }
            }
        }
    }
}