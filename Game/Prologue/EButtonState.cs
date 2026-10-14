using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Prologue
{
    public enum EButtonState
    {
        /// <summary>
        /// 继续
        /// </summary>
        [LabelText("继续")]
        Continue = 310000,

        /// <summary>
        /// 动画中
        /// </summary>
        [LabelText("动画中")]
        Tweening = 310009,

        /// <summary>
        /// 前进
        /// </summary>
        [LabelText("前进")]
        GoAhead = 310001,

        /// <summary>
        /// 战斗
        /// </summary>
        [LabelText("战斗")]
        Combat = 310002,

        /// <summary>
        /// 前进中
        /// </summary>
        [LabelText("前进中")]
        GoAheading = 310003,

        /// <summary>
        /// 战斗中
        /// </summary>
        [LabelText("战斗中")]
        Combating = 310004,
    }
}