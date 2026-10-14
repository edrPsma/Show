using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IBuffFactory : ISystem
    {
        /// <summary>
        /// 创建Buff
        /// </summary>
        /// <param name="buffID">buffID</param>
        /// <param name="caster">施法者</param>
        /// <param name="owner">拥有者</param>
        /// <returns></returns>
        Buff Create(int buffID, EntityInfo caster, IEntity owner);

        /// <summary>
        /// 回收Buff
        /// </summary>
        /// <param name="buff"></param>
        void ReleaseBuff(Buff buff);

        /// <summary>
        /// 获取Buff执行器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBuffExcutor GetBuffExcutor(int type);

        /// <summary>
        /// 获取Buff执行器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBuffExcutor GetBuffExcutor(EBuffExcutorType type);
    }
}