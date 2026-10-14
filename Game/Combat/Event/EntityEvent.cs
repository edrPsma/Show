using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EntityEvent
    {
        /// <summary>
        /// 创建技能
        /// </summary>
        CreateSkill = 101,

        /// <summary>
        /// 技能动作完成
        /// </summary>
        SkillActionDone = 105,

        /// <summary>
        /// 逻辑帧循环
        /// </summary>
        Update = 305,

        /// <summary>
        /// 攻击前事件，带动作
        /// </summary>
        BeforeAttackWithAction = 110,

        /// <summary>
        /// 单位死亡
        /// </summary>
        Death = 201,

        /// <summary>
        /// 单位回收
        /// </summary>
        Recycle = 301,
    }

    /// <summary>
    /// 英雄血量变化(界面显示)
    /// </summary>
    public struct EventHeroHpChange
    {
        /// <summary>
        /// 当前血量
        /// </summary>
        public long CurrentHP;

        /// <summary>
        /// 最大血量
        /// </summary>
        public long MaxHP;
    }

    /// <summary>
    /// 属性变化事件
    /// </summary>
    public struct EventAttributeChange
    {
        /// <summary>
        /// 属性变化量字典
        /// </summary>
        public Dictionary<int, long> ChangeDic;
    }

    /// <summary>
    /// 宠物属性变化事件
    /// </summary>
    public struct EventPetAttributeChange
    {
        public int PetID;

        public Dictionary<int, long> ChangeDic;
    }

    /// <summary>
    /// 宠物更改事件，包括阵型变化和形态变化
    /// </summary>
    public struct EventPetChange
    {

    }

    /// <summary>
    /// 装备修改事件
    /// </summary>
    public struct EventEquipChange
    {
        /// <summary>
        /// 装备ID
        /// </summary>
        public int EquipID;
    }

    /// <summary>
    /// 坐骑修改
    /// </summary>
    public struct EventChangeMounts
    {
        public int MountID;
    }

    /// <summary>
    /// 翅膀修改
    /// </summary>
    public struct EventChangeWing
    {
        public int WingID;
    }
}