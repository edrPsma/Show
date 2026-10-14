using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public enum ESelectType
    {
        /// <summary>
        /// 单个目标
        /// </summary>
        [LabelText("单个目标")]
        SingleTarget = 1,

        /// <summary>
        /// 范围
        /// </summary>
        [LabelText("范围")]
        AOE = 2,

        /// <summary>
        /// 锁定玩家
        /// </summary>
        [LabelText("锁定英雄")]
        Hero = 3,
    }
}