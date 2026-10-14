using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EBuffExcutorType
    {
        /// <summary>
        /// 属性修改
        /// </summary>
        AttributeModification = 1,

        /// <summary>
        /// 普通攻击伤害
        /// </summary>
        SimpleAttackDamage = 2,

        /// <summary>
        /// 技能伤害
        /// </summary>
        SkillDamage = 3,

        /// <summary>
        /// 受到伤害
        /// </summary>
        GetHurt = 4,

        /// <summary>
        /// 持续伤害
        /// </summary>
        Dot = 6,

        /// <summary>
        /// 眩晕 无法移动和释放技能
        /// </summary>
        Dizziness = 7,

        /// <summary>
        /// 额外持续时间
        /// </summary>
        ExtraDuration = 9,

        /// <summary>
        /// 免疫某类buff
        /// </summary>
        Immune = 10,

        /// <summary>
        /// 添加护盾
        /// </summary>
        AddShields=11,
    }
}