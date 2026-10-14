using System;
using System.Collections;
using System.Collections.Generic;
using Ev;
using UnityEngine;

namespace Combat
{
    public interface IEntity : IController
    {
        /// <summary>
        /// 单位事件源
        /// </summary>
        IEventSource EventSource { get; }

        /// <summary>
        /// 游戏物体
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// 位置
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 单位类型
        /// </summary>
        EEntityType EntityType { get; set; }

        /// <summary>
        /// 碰撞盒
        /// </summary>
        CapsuleCollider HitBox { get; }

        /// <summary>
        /// 单位ID
        /// </summary>
        int EntityID { get; }

        /// <summary>
        /// 职业
        /// </summary>
        int Carrer { get; }

        /// <summary>
        /// 唯一Id
        /// </summary>
        int GUID { get; }

        /// <summary>
        /// 是否激活
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// 是否为玩家单位
        /// </summary>
        bool IsPlayer { get; }

        /// <summary>
        /// 是否为怪物单位
        /// </summary>
        bool IsMonster { get; }

        /// <summary>
        /// 方向
        /// </summary>
        int Direction { get; }

        /// <summary>
        /// 属性集
        /// </summary>
        IAttributeSet AttributeSet { get; }

        /// <summary>
        /// AI代理
        /// </summary>
        IAIAgent AIAgent { get; }

        /// <summary>
        /// 动画控制器
        /// </summary>
        IAnimationController AnimationController { get; }

        /// <summary>
        /// 技能持有者
        /// </summary>
        ISkillOwner SkillOwner { get; }

        /// <summary>
        /// Buff持有者
        /// </summary>
        IBuffOwner BuffOwner { get; }

        /// <summary>
        /// 黑板
        /// </summary>
        Blackboard Blackboard { get; }

        /// <summary>
        /// 召唤物管理器
        /// </summary>
        SummonManager SummonManager { get; }

        /// <summary>
        /// 战斗类型
        /// </summary>
        ECombatType CombatType { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 计算伤害前事件
        /// </summary>
        EventBeforeCalDamage EventBeforeCalDamage { get; set; }

        /// <summary>
        /// 计算伤害后事件
        /// </summary>
        EventAfterCalDamage EventAfterCalDamage { get; set; }

        /// <summary>
        /// 造成伤害事件
        /// </summary>
        EventOnTakeDamage EventOnTakeDamage { get; set; }

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="direction"></param>
        void SetDirection(int direction);

        /// <summary>
        /// 逻辑帧循环
        /// </summary>
        /// <param name="deltaTime"></param>
        void Excute(float deltaTime);
    }

    public static class EntityExtra
    {
        public static bool IsHero(this IEntity entity)
        {
            return entity.EntityType == EEntityType.Hero || entity.EntityType == EEntityType.Pvp || entity.EntityType == EEntityType.Doppelganger;
        }
    }
}