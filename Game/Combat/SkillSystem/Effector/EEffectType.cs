using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum EEffectType
    {
        /// <summary>
        /// 造成伤害，默认类型，每个技能必有
        /// </summary>
        Damage = 0,

        /// <summary>
        /// 添加Buff
        /// </summary>
        AddBuff = 1,

        /// <summary>
        /// 斩杀
        /// </summary>
        Beheaded = 2,

        /// <summary>
        /// 普通攻击吸血
        /// </summary>
        NormalAttackBloodsucking = 5,

        /// <summary>
        /// 必中
        /// </summary>
        DefinitelyHit = 6,

        /// <summary>
        /// 伤害翻倍
        /// </summary>
        DamageModification = 7,

        /// <summary>
        /// 额外攻击
        /// </summary>
        ExtraAttack = 8,

        /// <summary>
        /// 伤害提高
        /// </summary>
        DamageIncrease = 9,

        /// <summary>
        /// 减少其他技能cd
        /// </summary>
        ReduceCd = 10,

        /// <summary>
        /// 驱散buff
        /// </summary>
        Disperse = 11,

        /// <summary>
        /// 治疗
        /// </summary>
        Cure = 12,

        /// <summary>
        /// 召唤单位
        /// </summary>
        Summon = 13,

        /// <summary>
        /// 召唤分身
        /// </summary>
        Doppelganger = 14,

        /// <summary>
        /// 多段伤害修改
        /// </summary>
        MultistageDamage = 15,
    }
}