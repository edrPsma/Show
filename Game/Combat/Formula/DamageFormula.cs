using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public static partial class DamageFormula
    {
        public static bool IsPVP { get; set; }
        static float basicAttackInterval = 1;

        /// <summary>
        /// 【A基础伤害计算】=（我方【D面板攻击】- 敌方【E面板防御】x【G破防系数】）*技能系数
        /// 公式取值范围：最小值为1
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="skillMul"></param>
        /// <returns></returns>
        private static double A(IAttributeSet attacker, IAttributeSet target, float skillMul, bool isPet)
        {
            double a = (D(attacker, isPet) - E(target) * G(attacker, target)) * skillMul;

            return Clamp1(a);
        }

        /// <summary>
        /// 【B基础伤害系数】=（【H暴击概率】？【I暴击伤害】）*【J伤害系数】
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="damageType"></param>
        /// <param name="isCritical"></param>
        /// <returns></returns>
        private static double B(IAttributeSet attacker, IAttributeSet target, EDamageType damageType, bool isCritical)
        {
            return (isCritical ? I(attacker) : 1) * J(attacker, target, damageType);
        }

        private static double C_PVP(IAttributeSet attacker, IAttributeSet target, ECareerType attackerCareerType, ECareerType targetCareerType)
        {
            return K(attacker, target, attackerCareerType, targetCareerType);
        }

        private static double C_PVE(IAttributeSet attacker, IAttributeSet target, EMonsterType? monsterType)
        {
            if (monsterType == null)
            {
                return 1;
            }
            else
            {
                return L(attacker, target, (EMonsterType)monsterType);
            }
        }

        private static DamageInfo CalPVPDamage(Randomizer randomizer, EntityInfo attacker, EntityInfo target, EDamageType damageType, float skillMul)
        {
            if (!attacker.IsValid || !target.IsValid)
            {
                return default;
            }
            else
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.IsDodge = IsDodge(target.Entity.AttributeSet, randomizer);
                damageInfo.IsDot = damageType == EDamageType.SustainedDamage;
                damageInfo.IsCritical = IsCritical(attacker.Entity.AttributeSet, target.Entity.AttributeSet, randomizer);
                damageInfo.IsValid = true;
                bool isPet = attacker.Entity.EntityType == EEntityType.Entourage;

                double m = A(attacker.Entity.AttributeSet, target.Entity.AttributeSet, skillMul, isPet)
                    * B(attacker.Entity.AttributeSet, target.Entity.AttributeSet, damageType, false)
                    * C_PVP(attacker.Entity.AttributeSet, target.Entity.AttributeSet, GetCareerType(ref attacker), GetCareerType(ref target))
                    + Z(attacker.Entity.AttributeSet, target.Entity.AttributeSet);

                double criticalM = A(attacker.Entity.AttributeSet, target.Entity.AttributeSet, skillMul, isPet)
                    * B(attacker.Entity.AttributeSet, target.Entity.AttributeSet, damageType, true)
                    * C_PVP(attacker.Entity.AttributeSet, target.Entity.AttributeSet, GetCareerType(ref attacker), GetCareerType(ref target))
                    + Z(attacker.Entity.AttributeSet, target.Entity.AttributeSet);


                damageInfo.DamageValue = CombatUtility.CeilToLong(m);
                damageInfo.CriticalDamageValue = CombatUtility.CeilToLong(criticalM);

                if (damageInfo.IsDodge)
                {
                    damageInfo.FinalDamageValue = 0;
                }
                else if (damageInfo.IsCritical)
                {
                    damageInfo.FinalDamageValue = damageInfo.CriticalDamageValue;
                }
                else
                {
                    damageInfo.FinalDamageValue = damageInfo.DamageValue;
                }

                return damageInfo;
            }
        }

        private static DamageInfo CalPVEDamage(Randomizer randomizer, EntityInfo attacker, EntityInfo target, EDamageType damageType, float skillMul)
        {
            if (!attacker.IsValid || !target.IsValid)
            {
                return default;
            }
            else
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.IsDodge = IsDodge(target.Entity.AttributeSet, randomizer);
                damageInfo.IsDot = damageType == EDamageType.SustainedDamage;
                damageInfo.IsCritical = IsCritical(attacker.Entity.AttributeSet, target.Entity.AttributeSet, randomizer);
                damageInfo.IsValid = true;
                bool isPet = attacker.Entity.EntityType == EEntityType.Entourage;

                // 未暴击伤害
                double m = A(attacker.Entity.AttributeSet, target.Entity.AttributeSet, skillMul, isPet)
                    * B(attacker.Entity.AttributeSet, target.Entity.AttributeSet, damageType, false)
                    * C_PVE(attacker.Entity.AttributeSet, target.Entity.AttributeSet, GetMonsterType(ref attacker, ref target))
                    + Z(attacker.Entity.AttributeSet, target.Entity.AttributeSet);

                // 暴击伤害
                double criticalM = A(attacker.Entity.AttributeSet, target.Entity.AttributeSet, skillMul, isPet)
                    * B(attacker.Entity.AttributeSet, target.Entity.AttributeSet, damageType, true)
                    * C_PVE(attacker.Entity.AttributeSet, target.Entity.AttributeSet, GetMonsterType(ref attacker, ref target))
                    + Z(attacker.Entity.AttributeSet, target.Entity.AttributeSet);

                damageInfo.DamageValue = CombatUtility.CeilToLong(m);
                damageInfo.CriticalDamageValue = CombatUtility.CeilToLong(criticalM);

                if (damageInfo.IsDodge)
                {
                    damageInfo.FinalDamageValue = 0;
                }
                else if (damageInfo.IsCritical)
                {
                    damageInfo.FinalDamageValue = damageInfo.CriticalDamageValue;
                }
                else
                {
                    damageInfo.FinalDamageValue = damageInfo.DamageValue;
                }

                return damageInfo;
            }
        }

        public static DamageInfo CalDamage(Randomizer randomizer, EntityInfo attacker, EntityInfo target, EDamageType damageType, float skillMul)
        {
            if (IsPVP)
            {
                return CalPVPDamage(randomizer, attacker, target, damageType, skillMul);
            }
            else
            {
                return CalPVEDamage(randomizer, attacker, target, damageType, skillMul);
            }
        }

        private static EMonsterType? GetMonsterType(ref EntityInfo entity1, ref EntityInfo entity2)
        {
            if (entity1.Entity.IsPlayer && entity2.Entity.IsPlayer)
            {
                return null;
            }
            else if (!entity1.Entity.IsPlayer)
            {
                return GetMonsterType(ref entity1);
            }
            else
            {
                return GetMonsterType(ref entity2);
            }
        }

        private static EMonsterType GetMonsterType(ref EntityInfo entity)
        {
            if (entity.Entity.IsMonster)
            {
                var data = DataTable.GetItem<DTMonster>(entity.Entity.EntityID);
                switch (data.Type)
                {
                    case DTMonster.EType.Pu_tong_guai: return EMonsterType.TrashFish;
                    case DTMonster.EType.B_o_s_s: return EMonsterType.Boss;
                    case DTMonster.EType.Tuan_dui_b_o_s_s: return EMonsterType.Boss;
                    default: return EMonsterType.TrashFish;
                }
            }
            else
            {
                Debug.LogError("Combat 非怪物单位参与了PVE伤害计算");
                return EMonsterType.TrashFish;
            }
        }

        private static ECareerType GetCareerType(ref EntityInfo entity)
        {
            var data = DataTable.GetItem<DTPlayer>(entity.Entity.EntityID);

            if (data == null) return ECareerType.None;

            switch (data.Career)
            {
                case DTPlayer.ECareer.Shi_lai_mu: return ECareerType.Warrior;
                case DTPlayer.ECareer.Zhan_shi: return ECareerType.Warrior;
                case DTPlayer.ECareer.Qi_shi: return ECareerType.Warrior;
                case DTPlayer.ECareer.Shou_hu_zhe: return ECareerType.Warrior;
                case DTPlayer.ECareer.Shou_hu_shen: return ECareerType.Warrior;
                case DTPlayer.ECareer.Jiao_dou_shi: return ECareerType.Warrior;
                case DTPlayer.ECareer.Kuang_zhan_shi: return ECareerType.Warrior;
                case DTPlayer.ECareer.Bai_zhan_zhi_hun: return ECareerType.Warrior;
                case DTPlayer.ECareer.Gong_shou: return ECareerType.Shooter;
                case DTPlayer.ECareer.Shou_mo: return ECareerType.Shooter;
                case DTPlayer.ECareer.Lie_mo_zhi_xin: return ECareerType.Shooter;
                case DTPlayer.ECareer.Lie_ren: return ECareerType.Shooter;
                case DTPlayer.ECareer.Gui_lie: return ECareerType.Shooter;
                case DTPlayer.ECareer.Ying_jie_mo_lie: return ECareerType.Shooter;
                case DTPlayer.ECareer.Fa_shi: return ECareerType.Mage;
                case DTPlayer.ECareer.Mu_shi: return ECareerType.Mage;
                case DTPlayer.ECareer.Zhu_ji: return ECareerType.Mage;
                case DTPlayer.ECareer.Jiu_shu_tian_shi: return ECareerType.Mage;
                case DTPlayer.ECareer.Wu_shi: return ECareerType.Mage;
                case DTPlayer.ECareer.Yuan_su_ling_zhu: return ECareerType.Mage;
                case DTPlayer.ECareer.Yuan_su_zhu_zai: return ECareerType.Mage;
                default: return ECareerType.Warrior;
                case DTPlayer.ECareer.Shen_she: return ECareerType.Shooter;
            }
        }
    }

    public struct DamageInfo
    {
        /// <summary>
        /// 伤害信息是否有效的
        /// </summary>
        public bool IsValid;

        /// <summary>
        /// 是否闪避
        /// </summary>
        public bool IsDodge;

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCritical;

        /// <summary>
        /// 是否为持续伤害
        /// </summary>
        public bool IsDot;

        /// <summary>
        /// 最终伤害数值
        /// </summary>
        public long FinalDamageValue;

        /// <summary>
        /// 未暴击伤害数值
        /// </summary>
        public long DamageValue;

        /// <summary>
        /// 暴击伤害数值
        /// </summary>
        public long CriticalDamageValue;
    }
}