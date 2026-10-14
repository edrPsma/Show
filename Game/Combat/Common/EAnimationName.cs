using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EAnimationName
    {
        /// <summary>
        /// 无
        /// </summary>
        none = 0,
        /// <summary>
        /// 闲置
        /// </summary>
        idle = 1,

        /// <summary>
        /// 移动
        /// </summary>
        run = 2,

        /// <summary>
        /// 攻击
        /// </summary>
        attack = 3,

        /// <summary>
        /// 死亡
        /// </summary>
        death = 5,

        /// <summary>
        /// 受击
        /// </summary>
        hit = 6,

        attack1 = 10,
        attack2 = 11,
    }
}