using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public static partial class DamageFormula
    {
        /// <summary>
        /// 【D面板攻击】= 攻击力*（ 1 + 攻击力加成/10000）*（ 1 + 技能攻击力加成/10000）
        /// 公式取值范围：最小值为1
        /// </summary>
        /// <returns></returns>
        public static double D(IAttributeSet target, bool isPet)
        {
            long atk = target.GetValue(EAttributeKey.AttackPower);
            long atkBonus = target.GetValue(EAttributeKey.AttackPowerBonus);
            long atkSkillBonus = target.GetValue(EAttributeKey.SkillAttackPowerBonus);
            long atkPetBonus = target.GetValue(EAttributeKey.PetAttackPowerBonus);
            double d = atk * (1 + atkBonus * 1d / 10000) * (1 + atkSkillBonus * 1d / 10000) * (1 + atkPetBonus * 1d / 10000);

            return Clamp1(d);
        }

        /// <summary>
        /// 【E面板防御】= 防御力*（ 1 + 防御力加成/10000）*（ 1 + 技能防御力加成/10000）
        /// 公式取值范围：无范围
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double E(IAttributeSet target)
        {
            long def = target.GetValue(EAttributeKey.Defense);
            long defBonus = target.GetValue(EAttributeKey.DefenseBonus);
            long defSkillBonus = target.GetValue(EAttributeKey.SkillDefenseBonus);
            double e = def * (1 + defBonus * 1d / 10000) * (1 + defSkillBonus * 1d / 10000);

            return e;
        }

        public static long CalFinal(long basic, long bonus, long skillBonus)
        {
            double e = basic * (1 + bonus * 1d / 10000) * (1 + skillBonus * 1d / 10000);

            return CombatUtility.CeilToLong(e);
        }

        /// <summary>
        /// 【F面板生命】= 生命值*（ 1 + 生命值加成/10000）*（ 1 + 技能生命值加成/10000）
        /// 公式取值范围：最小值为1
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double F(IAttributeSet target)
        {
            long hp = target.GetValue(EAttributeKey.Health);
            long hpBonus = target.GetValue(EAttributeKey.HealthBonus);
            long hpSkillBonus = target.GetValue(EAttributeKey.SkillHealthBonus);
            double f = hp * (1 + hpBonus * 1d / 10000) * (1 + hpSkillBonus * 1d / 10000);

            return Clamp1(f);
        }

        /// <summary>
        /// 计算生命值
        /// </summary>
        /// <param name="basic"></param>
        /// <param name="hpBonus"></param>
        /// <param name="skillHpBonus"></param>
        /// <returns></returns>
        public static double CalMaxHP(long basic, long hpBonus, long skillHpBonus)
        {
            return basic * (1 + hpBonus * 1d / 10000) * (1 + skillHpBonus * 1d / 10000);
        }

        /// <summary>
        /// 计算生命值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static long CalMaxHP(IAttributeSet target)
        {
            return CombatUtility.CeilToLong(F(target));
        }

        /// <summary>
        /// 【G破防系数】= 1 -（我方【破防】- 敌方【破防】）*0.1%
        /// 公式取值范围：最小值为0%，最大值为50%
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double G(IAttributeSet attacker, IAttributeSet target)
        {
            long attackerBreakDef = attacker.GetValue(EAttributeKey.BreakDefense);
            long targetBreakDef = target.GetValue(EAttributeKey.BreakDefense);
            double g = 1 - (attackerBreakDef - targetBreakDef) * 0.001d;

            return Clamp(g, 0, 0.5f);
        }

        /// <summary>
        /// 【H暴击概率】= 我方【暴击率】/10000 - 敌方【暴击抗性】/10000
        /// 公式取值范围：最小值为0%，最大值为100%
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double H(IAttributeSet attacker, IAttributeSet target)
        {
            long criticalRate = attacker.GetValue(EAttributeKey.CriticalRate);
            long criticalResistance = target.GetValue(EAttributeKey.CriticalResistance);
            double h = criticalRate * 1d / 10000 - criticalResistance * 1d / 10000;

            return Clamp(h, 0, 1);
        }

        /// <summary>
        /// 【I暴击伤害】= 暴击伤害
        /// 公式取值范围：最小值为10%
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double I(IAttributeSet target)
        {
            double i = target.GetValue(EAttributeKey.CriticalDamage) * 1d / 10000;

            return Clamp(i, 0.1d, i);
        }

        /// <summary>
        /// 【J伤害系数】= 1 + 我方【伤害加成】/10000 - 敌方【伤害减免】/10000
        /// 公式取值范围：最小值为40%
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="target">受击者</param>
        /// <param name="damageType">伤害类型</param>
        /// <returns></returns>
        private static double J(IAttributeSet attacker, IAttributeSet target, EDamageType damageType)
        {
            long damageIncrease = attacker.GetValue(EAttributeKey.DamageIncrease);
            long extraIncrease = attacker.GetValue((int)damageType);
            long damageReduction = target.GetValue(EAttributeKey.DamageReduction);
            double j = 1 + (damageIncrease + extraIncrease) * 1d / 10000 - damageReduction * 1d / 10000;

            return Clamp(j, 0.4d, j);
        }

        /// <summary>
        /// 【K，PVP伤害系数】= 1 + 我方【对职业伤害加成】/10000 - 敌方【对职业伤害减免】/10000（限定PVP环境生效）
        /// 公式取值范围：最小值为40%
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="target">受击者</param>
        /// <param name="attackerCareerType">攻击者职业类型</param>
        /// <param name="targetCareerType">受击者职业类型</param>
        /// <returns></returns>
        private static double K(IAttributeSet attacker, IAttributeSet target, ECareerType attackerCareerType, ECareerType targetCareerType)
        {
            if (attackerCareerType == ECareerType.None || targetCareerType == ECareerType.None)
            {
                return 1;
            }

            long damageIncrease = attacker.GetValue((int)targetCareerType);
            long damageReduction = target.GetValue((int)attackerCareerType + 1);
            double k = 1 + damageIncrease * 1d / 10000 - damageReduction * 1d / 10000;

            return Clamp(k, 0.4f, k);
        }

        /// <summary>
        /// 【L怪物伤害系数】= 1 + 我方【对怪物伤害加成】/10000 - 敌方【对怪物伤害减免】/10000（限定PVE环境生效）
        /// 公式取值范围：最小值为40%
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double L(IAttributeSet attacker, IAttributeSet target, EMonsterType monsterType)
        {
            long monsterDamageBonus = attacker.GetValue((int)monsterType);
            long monsterDamageReduction = target.GetValue((int)monsterType + 1);
            double l = 1 + monsterDamageBonus * 1d / 10000 - monsterDamageReduction * 1d / 10000;

            return Clamp(l, 0.4d, l);
        }

        /// <summary>
        /// 【N闪避概率】=闪避/10000
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double N(IAttributeSet target)
        {
            return target.GetValue(EAttributeKey.Dodge);
        }

        /// <summary>
        /// 【Z伤害追加】=我方【伤害追加】 - 敌方【伤害抵抗】
        /// 公式取值范围：最小值为0
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private static double Z(IAttributeSet attacker, IAttributeSet target)
        {
            long damageAddition = attacker.GetValue(EAttributeKey.DamageAddition);
            long damageResistance = target.GetValue(EAttributeKey.DamageResistance);
            double z = damageAddition - damageResistance;

            return Clamp0(z);
        }
    }
}