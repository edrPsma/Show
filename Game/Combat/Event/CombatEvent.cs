using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 开始战斗事件
    /// </summary>
    public struct EventStartCombat
    {
        /// <summary>
        /// 战斗类型
        /// </summary>
        public ECombatType CombatType;
    }

    /// <summary>
    /// 停止战斗事件
    /// </summary>
    public struct EventStopCombat
    {
        /// <summary>
        /// 战斗类型
        /// </summary>
        public ECombatType CombatType;
    }

    public struct EventCombatVictory
    {
        /// <summary>
        /// 战斗类型
        /// </summary>
        public ECombatType CombatType;
    }

    public struct EventCombatFailure
    {
        /// <summary>
        /// 战斗类型
        /// </summary>
        public ECombatType CombatType;
    }

    /// <summary>
    /// 开始战斗计时事件
    /// </summary>
    public struct EventTimerStateChange
    {
        /// <summary>
        /// 战斗类型
        /// </summary>
        public ECombatType CombatType;

        /// <summary>
        /// 是否开启计时
        /// </summary>
        public bool StartTimer;

        /// <summary>
        /// 计时时间
        /// </summary>
        public float Time;
    }
}