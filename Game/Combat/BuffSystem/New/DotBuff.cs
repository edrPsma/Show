using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.Dot)]
    public class DotBuff : BuffExcutor
    {
        const float INTERVAL = 0.2f;

        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            int percentage = parms[0];// 攻击力加成万分比
            double atk = DamageFormula.D(buff.Caster.Entity.AttributeSet, buff.Owner.EntityType == EEntityType.Entourage);
            long addValue = CombatUtility.CeilToLong(atk * percentage * 0.0001f);
            buff.Blackboard.SetLongValue(typeof(DotBuff), (id, EAttributeKey.AttackPower), addValue);
        }

        protected override void OnUpdate(Buff buff, int id, List<int> parms)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(buff.Owner.CombatType);
            float timer = buff.Blackboard.GetFloatValue(typeof(DotBuff), id) - combatManager.GetDeltaTime();
            if (timer <= 0)
            {
                buff.Blackboard.SetFloatValue(typeof(DotBuff), id, timer + INTERVAL);

                long addValue = buff.Blackboard.GetLongValue(typeof(DotBuff), (id, EAttributeKey.AttackPower));

                buff.Owner.AttributeSet.AddHP(buff.Caster.Entity, -addValue);

                DamageInfo damageInfo = new DamageInfo
                {
                    IsValid = true,
                    IsDodge = false,
                    IsCritical = false,
                    IsDot = true,
                    FinalDamageValue = addValue
                };

                this.GetSystem<IDamageMarkFactory>().Show(buff.Owner.AnimationController.HeadTrans.position, buff.Owner.IsPlayer ? -1 : 1, ref damageInfo);
            }
            else
            {
                buff.Blackboard.SetFloatValue(typeof(DotBuff), id, timer);
            }
        }
    }
}