using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 单位工厂
    /// </summary>
    public interface IEntityFactory : ISystem
    {
        Transform SceneTrans { get; }
        /// <summary>
        /// 创建单位
        /// </summary>
        /// <param name="combatType">战斗类型</param>
        /// <param name="entityType">单位类型</param>
        /// <param name="entityId">单位Id</param>
        /// <param name="isPlayer">是否为玩家单位</param>
        /// <param name="buildInfo">用户数据</param>
        /// <returns></returns>
        IEntity Create(ECombatType combatType, EEntityType entityType, int entityId, bool isPlayer, BuildInfo buildInfo = default);

        /// <summary>
        /// 创建空单位
        /// </summary>
        /// <returns></returns>
        IEntity CreateEmpty();

        /// <summary>
        /// 回收单位
        /// </summary>
        /// <param name="entity">单位</param>
        void Release(IEntity entity);

        /// <summary>
        /// 设置移动节点
        /// </summary>
        /// <param name="movePoint"></param>
        void SetSceneTrans(Transform movePoint);
    }
}