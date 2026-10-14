using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public partial class DamageFormula
    {
        /// <summary>
        /// 是否暴击
        /// </summary>
        /// <returns></returns>
        private static bool IsCritical(IAttributeSet attacker, IAttributeSet target, Randomizer randomizer)
        {
            double weight = H(attacker, target) * 10000;
            int randomValue = randomizer.GetRandom(1, 10000);

            return weight >= randomValue;
        }

        /// <summary>
        /// 是否闪避
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static bool IsDodge(IAttributeSet target, Randomizer randomizer)
        {
            double weight = Clamp(N(target), 0, 5000);
            int randomValue = randomizer.GetRandom(1, 10000);

            return weight >= randomValue;
        }

        /// <summary>
        /// 计算攻击速度
        /// 最终攻击间隔 = 基础攻击间隔 /（1 + 攻击速度/10000）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float CalAttackSpeed(IAttributeSet target)
        {
            long atkSpeed = target.GetValue(EAttributeKey.AttackSpeed);

            return basicAttackInterval * 1f / (1 + atkSpeed * 1f / 10000);
        }

        /// <summary>
        /// 计算攻击速度
        /// </summary>
        /// <param name="attackSpeed"></param>
        /// <returns></returns>
        public static float CalAttackSpeed(long attackSpeed)
        {
            return basicAttackInterval * 1f / (1 + attackSpeed * 1f / 10000);
        }

        /// <summary>
        /// 计算移动速度
        /// 最终移动速度 = 基础移动速度*（1 + 移动速度加成/10000）
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static long CalMoveSpeed(IAttributeSet target)
        {
            long moveSpeedBonus = target.GetValue(EAttributeKey.MovementSpeedBonus);
            long moveSpeed = target.GetValue(EAttributeKey.MovementSpeed);

            return CombatUtility.CeilToLong(moveSpeed * (1 + moveSpeedBonus * 1d / 10000));
        }

        /// <summary>
        /// 计算移动速度
        /// </summary>
        /// <param name="moveSpeed">基础移动速度</param>
        /// <param name="moveSpeedBonus">移动速度加成</param>
        /// <returns></returns>
        public static long CalMoveSpeed(long moveSpeed, long moveSpeedBonus)
        {
            return CombatUtility.CeilToLong(moveSpeed * (1 + moveSpeedBonus * 1d / 10000));
        }

        /// <summary>
        /// 计算生命恢复
        /// 生命恢复总值 = 生命恢复*（1 + 生命恢复加成/10000）
        /// 公式取值范围：最小值为0
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static long CalLifeRegeneration(IAttributeSet target)
        {
            long healthRecovery = target.GetValue(EAttributeKey.HealthRecovery);
            long healthRecoveryBonus = target.GetValue(EAttributeKey.HealthRecoveryBonus);
            double value = healthRecovery * (1 + healthRecoveryBonus * 1d / 10000);

            return CombatUtility.CeilToLong(Clamp0(value));
        }

        /// <summary>
        /// 计算治疗总值
        /// 治疗总值 = 基础治疗量*（1 + 治疗加成/10000）
        /// </summary>
        /// <param name="target"></param>
        /// <param name="healingAmount"></param>
        /// <returns></returns>
        public static long CalHealingValue(IAttributeSet target, float healingAmount)
        {
            long healingBonus = target.GetValue(EAttributeKey.HealingBonus);
            double value = healingAmount * (1 + healingBonus * 1d / 10000);

            return CombatUtility.CeilToLong(Clamp0(value));
        }

        /// <summary>
        /// 计算吸血值
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="damage">M伤害大结算</param>
        /// <returns></returns>
        public static long CalLifestealValue(IAttributeSet attacker, double damage)
        {
            double value = attacker.GetValue(EAttributeKey.Lifesteal) * damage * 1d / 10000;

            return CombatUtility.CeilToLong(value);
        }
    }
}