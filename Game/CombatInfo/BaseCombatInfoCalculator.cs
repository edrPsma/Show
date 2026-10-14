using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Combat.CombatInformation
{
    public class BaseCombatInfoCalculator : Controller
    {
        /// <summary>
        /// 计算本地属性
        /// </summary>
        /// <param name="list"></param>
        public virtual void CalculatingLocalProperty(List<PropertyInfo> list) { }

        /// <summary>
        /// 计算网络属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="data"></param>
        public virtual void CalculatingNetProperty(List<PropertyInfo> list, MSG_Detail data) { }

        /// <summary>
        /// 计算本地技能
        /// </summary>
        /// <param name="skillList"></param>
        public virtual void CalculatingLocalSkill(List<SkillLevelInfo> skillList) { }

        /// <summary>
        /// 计算网络技能
        /// </summary>
        /// <param name="skillList"></param>
        /// <param name="data"></param>
        public virtual void CalculatingNetSkill(List<SkillLevelInfo> skillList, MSG_Detail data) { }

        /// <summary>
        /// 计算本地宠物属性
        /// </summary>
        /// <param name="petID"></param>
        /// <param name="list"></param>
        public virtual void CalculatingLocalPetProperty(int petID, List<PropertyInfo> list) { }

        /// <summary>
        /// 计算网络宠物属性
        /// </summary>
        /// <param name="list"></param>
        /// <param name="data"></param>
        public virtual void CalculatingNetPetProperty(List<PropertyInfo> list, MSG_DetailPet data) { }
    }
}