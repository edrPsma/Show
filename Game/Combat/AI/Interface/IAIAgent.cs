using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Combat
{
    public interface IAIAgent
    {
        /// <summary>
        /// 是否停止AI
        /// </summary>
        public static bool StopAI { get; set; }

        /// <summary>
        /// AI是否启用
        /// </summary>
        bool Active { get; }
        /// <summary>
        /// 是否死亡
        /// </summary>
        bool IsDeath { get; }

        /// <summary>
        /// 不能行动
        /// </summary>
        ClampInt CantDoAction { get; set; }

        /// <summary>
        /// 不能移动
        /// </summary>
        ClampInt CantMove { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        EntityInfo Target { get; }

        /// <summary>
        /// 行为树
        /// </summary>
        BehaviorTree BehaviorTree { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="setBehaviour"></param>
        void Init(ExternalBehavior behavior);

        /// <summary>
        /// 设置目标
        /// </summary>
        /// <param name="entity">目标对象</param>
        void SetTarget(IEntity entity);

        /// <summary>
        /// 开始行为
        /// </summary>
        void StartBehavior();

        /// <summary>
        /// 停止行为
        /// </summary>
        void StopBehavior();
    }
}