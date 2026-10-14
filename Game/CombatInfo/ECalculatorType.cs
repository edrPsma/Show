using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.CombatInformation
{
    public enum ECalculatorType
    {
        /// <summary>
        /// 基础
        /// </summary>
        Basis = 0,

        /// <summary>
        /// 装备
        /// </summary>
        Equip = 1,

        /// <summary>
        /// 科技园
        /// </summary>
        SciencePark = 2,

        /// <summary>
        /// 宠物阵法
        /// </summary>
        PetFormation = 3,

        /// <summary>
        /// 坐骑
        /// </summary>
        Mount = 4,

        /// <summary>
        /// 神器
        /// </summary>
        Divine = 5,

        /// <summary>
        /// 翅膀
        /// </summary>
        Wing = 6,

        /// <summary>
        /// 徽章
        /// </summary>
        Badge = 7,

        /// <summary>
        /// 圣物
        /// </summary>
        Halidom = 8,

        /// <summary>
        /// 图鉴
        /// </summary>
        Illustration = 9,

        /// <summary>
        /// 公会天赋树
        /// </summary>
        GuildHeartLaw = 10,
    }
}