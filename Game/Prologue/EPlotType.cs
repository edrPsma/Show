using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Prologue
{
    [Flags]
    public enum EPlotType
    {
        [LabelText("无")]
        None = 0,

        [LabelText("对话")]
        Dialogue = 1 << 0,

        [LabelText("移动")]
        Move = 1 << 1,

        [LabelText("学习技能")]
        LearnSkill = 1 << 2,

        [LabelText("等待一段时间")]
        Wait = 1 << 3,

        [LabelText("开始战斗")]
        Combat = 1 << 4,

        [LabelText("生成单位")]
        SpawnEntity = 1 << 5,

        [LabelText("TimeLine")]
        TimeLine = 1 << 6,

        [LabelText("设置AI状态")]
        AIState = 1 << 7,

        [LabelText("移除单位")]
        RemoveEntity = 1 << 8,
    }
}