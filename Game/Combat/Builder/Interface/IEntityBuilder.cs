using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IEntityBuilder
    {
        /// <summary>
        /// 单位构建
        /// </summary>
        /// <param name="factory">工厂</param>
        /// <param name="entity">单位</param>
        /// <param name="entityId">单位id</param>
        /// <param name="isPlayer">是否为玩家阵营</param>
        /// <param name="userData">用户数据</param>
        void Build(IEntityFactory factory, Entity entity, int entityId, bool isPlayer, BuildInfo userData);
    }
}