using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public static partial class DamageFormula
    {
        /// <summary>
        /// 钳制最小值为0
        /// </summary>
        /// <param name="value">当前值</param>
        /// <returns></returns>
        public static double Clamp0(double value)
        {
            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        /// <summary>
        /// 钳制最小值为1
        /// </summary>
        /// <param name="value">当前值</param>
        /// <returns></returns>
        public static double Clamp1(double value)
        {
            if (value < 1)
            {
                return 1;
            }

            return value;
        }

        /// <summary>
        /// 钳制
        /// </summary>
        /// <param name="value">当前值</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }

        public enum EDamageType
        {
            /// <summary>
            /// 普通攻击
            /// </summary>
            NormalAttack = 204,

            /// <summary>
            /// 技能
            /// </summary>
            Skill = 205,

            /// <summary>
            /// 持续伤害
            /// </summary>
            SustainedDamage = 206
        }

        public enum ECareerType
        {
            None = 0,

            /// <summary>
            /// 战士
            /// </summary>
            Warrior = 304,

            /// <summary>
            /// 射手
            /// </summary>
            Shooter = 306,

            /// <summary>
            /// 法师
            /// </summary>
            Mage = 308
        }

        public enum EMonsterType
        {
            /// <summary>
            /// 小怪
            /// </summary>
            TrashFish = 310,

            /// <summary>
            /// Boss
            /// </summary>
            Boss = 312
        }
    }
}