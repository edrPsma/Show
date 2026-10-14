using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Combat
{
    public interface ISkillExcutor
    {
        bool Active { get; }

        /// <summary>
        /// 锁定目标
        /// </summary>
        EntityInfo LockTarget { get; }

        /// <summary>
        /// 技能信息
        /// </summary>
        SkillInfo SkillInfo { get; }

        /// <summary>
        /// 位置
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// 旋转
        /// </summary>
        Quaternion Rotation { get; }

        /// <summary>
        /// 缩放
        /// </summary>
        Vector3 Scale { get; }

        ECombatType CombatType { get; set; }

        /// <summary>
        /// new 方法
        /// </summary>
        /// <returns></returns>
        ISkillExcutor New();

        /// <summary>
        /// 初始化
        /// </summary>
        void Start(ECombatType combatType, EntityInfo lockTarget, SkillInfo skillInfo);

        /// <summary>
        /// 逻辑帧更新
        /// </summary>
        void Update(float deltaTime);

        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();

        /// <summary>
        /// 设置位置
        /// </summary>
        void SetPosition(Vector3 position);

        /// <summary>
        /// 设置旋转
        /// </summary>
        void SetRotation(Quaternion rotation);

        /// <summary>
        /// 设置缩放
        /// </summary>
        void SetScale(Vector3 scale);
    }
}