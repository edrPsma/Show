using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    public interface ISkillFactory : ISystem
    {
        /// <summary>
        /// 获取选择器
        /// </summary>
        /// <param name="type">选择器类型</param>
        /// <returns></returns>
        ISelector GetSelector(ESelectType type);

        /// <summary>
        /// 获取技能执行器类型
        /// </summary>
        /// <param name="skillInfo">技能信息</param>
        /// <returns></returns>
        Type GetSkillExcutorType(SkillInfo skillInfo);


        /// <summary>
        /// 回收技能信息
        /// </summary>
        /// <param name="skillInfo"></param>
        void RecycleSkillInfo(SkillInfo skillInfo);

        /// <summary>
        /// 回收技能信息
        /// </summary>
        /// <param name="skillInfos"></param>
        void RecycleSkillInfos(List<SkillInfo> skillInfos);

        /// <summary>
        /// 产出技能信息
        /// </summary>
        /// <param name="entity">单位信息</param>
        /// <param name="skillId">技能ID</param>
        /// <returns></returns>
        SkillInfo SpawnSkillInfo(EntityInfo entityInfo, int skillId);

        /// <summary>
        /// 绑定技能
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="skillIds"></param>
        void BindSkill(Entity entity, int[] skillIds);

        /// <summary>
        /// 获取技能效果器
        /// </summary>
        /// <param name="skillInfo"></param>
        /// <param name="effectors"></param>
        void GetSkillEffector(SkillInfo skillInfo, List<EffectorItem> effectors);

        /// <summary>
        /// 回收效果器
        /// </summary>
        /// <param name="effectors"></param>
        void RecycleEffectorItems(List<EffectorItem> effectors);

        /// <summary>
        /// 设置技能层级
        /// </summary>
        /// <param name="skillInfo"></param>
        /// <param name="isPlayer"></param>
        void SetSkillLayer(SkillInfo skillInfo, bool isPlayer);

        /// <summary>
        /// 获取技能配置文件
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        SkillConfig GetSkillConfig(int skillId);
    }
}