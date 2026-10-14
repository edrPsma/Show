using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Combat.CombatInformation
{
    public class CombatInfo : Controller
    {
        /// <summary>
        /// 属性
        /// </summary>
        public List<PropertyInfo> Property;

        /// <summary>
        /// 技能列表
        /// </summary>
        public List<SkillLevelInfo> SkillList;

        /// <summary>
        /// 宠物列表
        /// </summary>
        public List<PetInfo> PetList;

        /// <summary>
        /// 主武器ID
        /// </summary>
        public int PrimaryWeaponID;

        /// <summary>
        /// 副武器ID
        /// </summary>
        public int SecondaryWeaponID;

        /// <summary>
        /// 翅膀ID
        /// </summary>
        public int WingID;

        /// <summary>
        /// 坐骑ID
        /// </summary>
        public int MountID;
    }

    public class PetInfo
    {
        /// <summary>
        /// 宠物ID
        /// </summary>
        public int PetID;

        /// <summary>
        /// 阶段
        /// </summary>
        public int Advanced;

        /// <summary>
        /// 属性
        /// </summary>
        public List<PropertyInfo> Property;
    }

    public class PropertyInfo
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 属性值
        /// </summary>
        public long Value;

        public PropertyInfo(int id, long value)
        {
            ID = id;
            Value = value;
        }

        public PropertyInfo(MSG_Property detailProperty)
        {
            ID = detailProperty.m_id;
            Value = detailProperty.m_value;
        }
    }

    public class SkillLevelInfo
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int Level;

        public SkillLevelInfo(int id, int level)
        {
            ID = id;
            Level = level;
        }

        public SkillLevelInfo(MSG_Skill detailSkill)
        {
            ID = detailSkill.m_id;
            Level = detailSkill.m_level;
        }
    }
}