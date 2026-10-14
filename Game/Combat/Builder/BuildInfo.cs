using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public struct BuildInfo
    {
        /// <summary>
        /// 怪物索引，服务器杀怪用
        /// </summary>
        public int MonsterIndex;

        #region 召唤物

        /// <summary>
        /// 召唤持续时间
        /// </summary>
        public float SummonDuration;

        /// <summary>
        /// 召唤者
        /// </summary>
        public IEntity Summoner;

        #endregion

        #region 宠物
        /// <summary>
        /// 宠物位置索引
        /// </summary>
        public int PetIndex;
        #endregion

        /// <summary>
        /// 是否发送刷新技能面板事件
        /// </summary>
        public bool RefreshSkillList;

        #region 分身

        /// <summary>
        /// 主武器ID
        /// </summary>
        public int PrimaryWeaponID;

        /// <summary>
        /// 副武器ID
        /// </summary>
        public int SecondaryWeaponID;
        #endregion
    }
}