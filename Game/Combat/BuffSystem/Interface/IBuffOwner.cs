using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IBuffOwner
    {
        /// <summary>
        /// 单位
        /// </summary>
        IEntity Entity { get; }

        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="buff"></param>
        void RemoveBuff(Buff buff);

        /// <summary>
        /// 添加Buff
        /// </summary>
        /// <param name="buffId">buffID</param>
        /// <param name="caster">施法者</param>
        /// <param name="multiple">倍率</param>
        /// <param name="skillID">技能ID</param>
        void AddBuff(int buffId, EntityInfo caster, float multiple, int skillID, int skillLevel);

        /// <summary>
        /// 驱散buff
        /// </summary>
        /// <param name="buffID"></param>
        void Disperse(int buffID);

        /// <summary>
        /// 将某类型加入到忽略列表
        /// </summary>
        /// <param name="tage"></param>
        void AddToIgnoreList(DTSkill_buff.ETage tage);

        /// <summary>
        /// 将某类型从忽略列表中移除
        /// </summary>
        /// <param name="tage"></param>
        void RemoveFormIgnoreList(DTSkill_buff.ETage tage);

        /// <summary>
        /// 获取所有buff
        /// </summary>
        /// <returns></returns>
        int[] GetAllBuff();

        /// <summary>
        /// 获取Buff
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        Buff GetBuff(int buffID);

        /// <summary>
        /// 逻辑帧执行
        /// </summary>
        /// <param name="deltaTime"></param>
        void Excute(float deltaTime);
    }
}