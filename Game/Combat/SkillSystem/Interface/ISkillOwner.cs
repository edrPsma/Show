using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface ISkillOwner
    {
        /// <summary>
        /// 技能列表
        /// </summary>
        List<SkillInfo> SkillInfos { get; }

        /// <summary>
        /// 当前要释放的技能
        /// </summary>
        SkillInfo ReleaseSkill { get; set; }

        /// <summary>
        /// 技能释放队列
        /// </summary>
        LinkedList<SkillInfo> ReleaseQueue { get; }

        /// <summary>
        /// 下一个要释放的技能
        /// </summary>
        SkillInfo NextSkill { get; }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillInfo"></param>
        void DoRelease(SkillInfo skillInfo);

        /// <summary>
        /// 设置技能冷却时间
        /// </summary>
        /// <param name="skillID"></param>
        /// <param name="time"></param>
        void SetSkillTimer(int skillID, float time);

        /// <summary>
        /// 获取技能冷却时间
        /// </summary>
        /// <param name="skillID"></param>
        /// <returns></returns>
        float GetSkillTimer(int skillID);

        /// <summary>
        /// 创建技能
        /// </summary>
        /// <param name="skillInfo"></param>
        /// <param name="lockTarget"></param>
        /// <param name="ignoreList"></param>
        void CreateSkill(SkillInfo skillInfo, EntityInfo lockTarget = default);

        /// <summary>
        /// 创建技能
        /// </summary>
        /// <param name="id"></param>
        void CreateSkill(int id);

        /// <summary>
        /// 逻辑帧循环
        /// </summary>
        /// <param name="deltaTime"></param>
        void Excute(float deltaTime);
    }
}