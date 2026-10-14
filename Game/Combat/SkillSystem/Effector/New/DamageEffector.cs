using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Damage)]
    public class DamageEffector : BaseEffector
    {
        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            excutor.DamageRatio = excutor.SkillInfo.Multiple;
            excutor.Duration = excutor.SkillInfo.Config.Duration;
            excutor.Interval = excutor.SkillInfo.Config.Interval;
            excutor.AttackTime = 0;
            excutor.AttackTimer = 0;
            excutor.DelayTimer = excutor.SkillInfo.Config.Delay;
        }

        protected override void OnExcute(SkillExcutor excutor, EntityInfo skillOwner, float deltaTime, List<int> param)
        {
            if (!excutor.DamageStart) return;

            excutor.DebugSkill();
            if (excutor.DelayTimer > 0)
            {
                excutor.DelayTimer -= deltaTime;
                return;
            }

            if (excutor.AttackTime < excutor.Duration || excutor.AttackTimer == 0)
            {
                if (excutor.AttackTime == 0 || excutor.AttackTimer >= excutor.Interval)
                {
                    excutor.SelectTargets();
                    switch (excutor.SkillInfo.Config.DestoryType)
                    {
                        case EDestoryType.Trigger:
                            if (excutor.Targets.Count > 0)
                            {
                                foreach (var entity in excutor.Targets)
                                {
                                    TakeDamage(excutor, excutor.SkillInfo.Owner, entity, excutor.SkillInfo);
                                }
                                excutor.Dispose();
                            }
                            break;
                        case EDestoryType.Time:
                            foreach (var entity in excutor.Targets)
                            {
                                TakeDamage(excutor, excutor.SkillInfo.Owner, entity, excutor.SkillInfo);
                            }
                            break;
                    }
                    if (excutor.AttackTime != 0)
                    {
                        excutor.AttackTimer -= excutor.Interval;
                    }
                }

                excutor.AttackTime += deltaTime;
                excutor.AttackTimer += deltaTime;
            }
            else
            {
                excutor.Dispose();
            }
        }


        private void AddHitEffect(SkillExcutor excutor, EntityInfo entity)
        {
            if (string.IsNullOrEmpty(excutor.SkillInfo.Config.HitPrefabPath)) return;

            ICombatSystem combatSystem = this.GetSystem<ICombatSystem>();
            CombatManager combatManager = combatSystem.GetCombatManager(excutor.CombatType);

            Vector3 pos = entity.Entity.AnimationController.BodyTrans.position;

            GameEntry.Prefab.Instantiate(excutor.SkillInfo.Config.HitPrefabPath, prefab => OnHitPrefabLoadOver(pos, combatManager, prefab), null, EPrefabTag.Combat);
        }

        private void OnHitPrefabLoadOver(Vector3 pos, CombatManager combatManager, GameObject prefab)
        {
            ParticleInformation information = prefab.GetOrAddComponent<ParticleInformation>();
            information.Play(combatManager.GameSpeed.Value);
            prefab.transform.position = pos;

            GameEntry.Task.AddTask(_ =>
            {
                if (GameEntry.Prefab.IsSpawnFromPool(prefab))
                {
                    GameEntry.Prefab.Release(prefab, true);
                }
            })
            .SetName("HitPrefab Recycle")
            .Delay(TimeSpan.FromSeconds(Mathf.Clamp(information.Duration, 0, 5) * 1f / GetGameSpeed(combatManager)))
            .Run();
        }

        double GetGameSpeed(CombatManager combatManager)
        {
            double value = combatManager.GameSpeed.Value;
            if (value == 0) return 5;
            return value;
        }

        void TakeAfterEffector(SkillExcutor excutor, EntityInfo attacker, EntityInfo target, ref DamageInfo damageInfo)
        {
            for (int i = 1; i < excutor.Effectors.Count; i++)
            {
                if (excutor.Effectors[i].TriggerTiming != DTSkill_effect.ETime.Gong_ji_hou) continue;

                excutor.Effectors[i].Effector?.As<ITakeDamageAfterEffector>()?.TakeEffect(excutor, attacker, target, ref damageInfo, excutor.Effectors[i].EffectValue);
            }
        }

        void TakeBeforeEffector(SkillExcutor excutor, EntityInfo attacker, EntityInfo target, ref DamageInfo damageInfo)
        {
            for (int i = 1; i < excutor.Effectors.Count; i++)
            {
                if (excutor.Effectors[i].TriggerTiming != DTSkill_effect.ETime.Gong_ji_qian) continue;

                excutor.Effectors[i].Effector?.As<ITakeDamageBeforeEffector>()?.TakeEffect(excutor, attacker, target, ref damageInfo, excutor.Effectors[i].EffectValue);
            }
        }

        void TakeDamage(SkillExcutor excutor, EntityInfo attacker, EntityInfo target, SkillInfo skillInfo)
        {
            if (!attacker.IsValid) return;
            if (!target.IsValid) return;
            if (!target.IsAlive) return;

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(excutor.CombatType);
            DamageInfo damageInfo = default;

            // 有先后顺序 效果器最先执行 目标的事件随后 最后是攻击方的事件
            TakeBeforeEffector(excutor, attacker, target, ref damageInfo);
            target.Entity.EventBeforeCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
            attacker.Entity.EventBeforeCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);

            // 如果效果器提供了一个合法的伤害信息，则不需要走伤害计算公式
            if (!damageInfo.IsValid)
            {
                damageInfo = DamageFormula.CalDamage(combatManager.Random, attacker, target, GetDamageType(skillInfo), excutor.DamageRatio);
            }

            // 有先后顺序 效果器最先执行 目标的事件随后 最后是攻击方的事件
            TakeAfterEffector(excutor, attacker, target, ref damageInfo);
            target.Entity.EventAfterCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
            attacker.Entity.EventAfterCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
            excutor.OnTakeDamage(target);

            // 如果伤害信息不合法 则不处理后续
            if (damageInfo.IsValid)
            {
                if (attacker.Entity.IsPlayer == target.Entity.IsPlayer)
                {
                    // 同一阵营不扣血
                }
                else
                {
                    this.GetSystem<ICombatSystem>().RecordDamage(damageInfo.FinalDamageValue);
                    target.Entity.AttributeSet.AddHP(attacker.Entity, -damageInfo.FinalDamageValue);

                    //吸血
                    long hp = DamageFormula.CalLifestealValue(attacker.Entity.AttributeSet, damageInfo.FinalDamageValue);
                    attacker.Entity.AttributeSet.AddHP(attacker.Entity, hp);
                    //TODO 回血字体

                    AddHitEffect(excutor, target);
                    //TODO 显示伤害字体
                    IDamageMarkFactory damageMarkFactory = this.GetSystem<IDamageMarkFactory>();
                    damageMarkFactory.Show(target.Entity.AnimationController.HeadTrans.position, target.Entity.IsPlayer ? -1 : 1, ref damageInfo);
                }
            }
        }

        DamageFormula.EDamageType GetDamageType(SkillInfo skillInfo)
        {
            switch (skillInfo.Data.SkillType)
            {
                case DTSkill_base.ESkillType.Zhi_ye_zhu_dong: return DamageFormula.EDamageType.Skill;
                case DTSkill_base.ESkillType.Zhi_ye_bei_dong: return DamageFormula.EDamageType.Skill;
                case DTSkill_base.ESkillType.Tong_yong_ji_neng: return DamageFormula.EDamageType.Skill;
                case DTSkill_base.ESkillType.Pu_tong_gong_ji: return DamageFormula.EDamageType.NormalAttack;
            }
            return DamageFormula.EDamageType.NormalAttack;
        }
    }
}