using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 选择器
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// 将在伤害范围内的单位选择出来
        /// </summary>
        /// <param name="info"></param>
        /// <param name="skillExcutor"></param>
        /// <param name="infos"></param>
        void Select(SkillInfo info, ISkillExcutor skillExcutor, HashSet<EntityInfo> infos);


        void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor);
    }
}