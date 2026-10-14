using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EAttributeKey
    {
        /// <summary>
        /// 生命值（整数）
        /// </summary>
        Health = 101,

        /// <summary>
        /// 攻击力（整数）
        /// </summary>
        AttackPower = 102,

        /// <summary>
        /// 防御力（整数）
        /// </summary>
        Defense = 103,

        /// <summary>
        /// 破防（整数）
        /// </summary>
        BreakDefense = 104,

        /// <summary>
        /// 攻击速度（整数）
        /// </summary>
        AttackSpeed = 105,

        /// <summary>
        /// 移动速度（整数）
        /// </summary>
        MovementSpeed = 106,

        /// <summary>
        /// 暴击率（百分比）
        /// </summary>
        CriticalRate = 107,

        /// <summary>
        /// 暴击伤害（百分比）
        /// </summary>
        CriticalDamage = 108,

        /// <summary>
        /// 生命恢复（整数）
        /// </summary>
        HealthRecovery = 109,

        /// <summary>
        /// 闪避（整数）
        /// </summary>
        Dodge = 110,

        /// <summary>
        /// 伤害追加（整数）
        /// </summary>
        DamageAddition = 111,

        /// <summary>
        /// 伤害抵抗（整数）
        /// </summary>
        DamageResistance = 112,

        /// <summary>
        /// 暴击抗性（整数）
        /// </summary>
        CriticalResistance = 113,

        /// <summary>
        /// 吸血（百分比）
        /// </summary>
        Lifesteal = 114,

        /// <summary>
        /// 伤害增加（百分比）
        /// </summary>
        DamageIncrease = 115,

        /// <summary>
        /// 伤害减免（百分比）
        /// </summary>
        DamageReduction = 116,

        /// <summary>
        /// 生命值加成（百分比）
        /// </summary>
        HealthBonus = 201,

        /// <summary>
        /// 攻击力加成（百分比）
        /// </summary>
        AttackPowerBonus = 202,

        /// <summary>
        /// 防御力加成（百分比）
        /// </summary>
        DefenseBonus = 203,

        /// <summary>
        /// 普攻伤害加成（百分比）
        /// </summary>
        BasicAttackDamageBonus = 204,

        /// <summary>
        /// 技能伤害加成（百分比）
        /// </summary>
        SkillDamageBonus = 205,

        /// <summary>
        /// 持续伤害加成（百分比）
        /// </summary>
        ContinuousDamageBonus = 206,

        /// <summary>
        /// 治疗加成（百分比）
        /// </summary>
        HealingBonus = 207,

        /// <summary>
        /// 生命恢复加成（百分比）
        /// </summary>
        HealthRecoveryBonus = 208,

        /// <summary>
        /// 移动速度加成（百分比）
        /// </summary>
        MovementSpeedBonus = 209,

        /// <summary>
        /// 技能生命值加成（百分比）
        /// </summary>
        SkillHealthBonus = 301,

        /// <summary>
        /// 技能攻击力加成（百分比）
        /// </summary>
        SkillAttackPowerBonus = 302,

        /// <summary>
        /// 技能防御力加成（百分比）
        /// </summary>
        SkillDefenseBonus = 303,

        /// <summary>
        /// 对战士伤害加成（百分比）
        /// </summary>
        WarriorDamageBonus = 304,

        /// <summary>
        /// 对战士伤害减免（百分比）
        /// </summary>
        WarriorDamageReduction = 305,

        /// <summary>
        /// 对射手伤害加成（百分比）
        /// </summary>
        ArcherDamageBonus = 306,

        /// <summary>
        /// 对射手伤害减免（百分比）
        /// </summary>
        ArcherDamageReduction = 307,

        /// <summary>
        /// 对法师伤害加成（百分比）
        /// </summary>
        MageDamageBonus = 308,

        /// <summary>
        /// 对法师伤害减免（百分比）
        /// </summary>
        MageDamageReduction = 309,

        /// <summary>
        /// 怪物伤害加成（百分比）
        /// </summary>
        MonsterDamageBonus = 310,

        /// <summary>
        /// 怪物伤害减免（百分比）
        /// </summary>
        MonsterDamageReduction = 311,

        /// <summary>
        /// BOSS伤害加成（百分比）
        /// </summary>
        BossDamageBonus = 312,

        /// <summary>
        /// BOSS伤害减免（百分比）
        /// </summary>
        BossDamageReduction = 313,

        /// <summary>
        /// 宠物攻击力加成（百分比）
        /// </summary>
        PetAttackPowerBonus = 314,

        /// <summary>
        /// 宠物攻速加成（百分比）
        /// </summary>
        PetAttackSpeedBonus = 315

    }
}

