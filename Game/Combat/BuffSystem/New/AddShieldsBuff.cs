using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.AddShields)]
    public class AddShieldsBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            int type = parms[0];

            long shieldValue = 0;
            if (type == 1)
            {
                long hpMax = DamageFormula.CalMaxHP(buff.Owner.AttributeSet);
                shieldValue = CombatUtility.CeilToLong((double)buff.Multiple * hpMax);// 护盾量
            }
            else if (type == 2)
            {
                long hpMax = DamageFormula.CalMaxHP(buff.Owner.AttributeSet);
                shieldValue = CombatUtility.CeilToLong(parms[2] * 0.0001d * hpMax);// 护盾量
            }

            buff.Blackboard.SetLongValue(typeof(AddShieldsBuff), id, shieldValue);
            buff.Owner.AttributeSet.AddShield(shieldValue);

            EventAfterCalDamage eventAfterCalDamage = (ref DamageInfo damageInfo, IEntity attacker, IEntity target, int skillID) =>
            {
                OnEventAfterCalDamage(ref damageInfo, target, skillID, buff, id);
            };
            buff.Blackboard.SetReferenceValue(typeof(AddShieldsBuff), id, eventAfterCalDamage);
            buff.Owner.EventAfterCalDamage += eventAfterCalDamage;
        }

        private void OnEventAfterCalDamage(ref DamageInfo damageInfo, IEntity target, int skillID, Buff buff, int id)
        {
            if (target != buff.Owner) return;

            long curShieldValue = buff.Blackboard.GetLongValue(typeof(AddShieldsBuff), id);

            // 护盾不足以抵扣当前伤害
            if (damageInfo.FinalDamageValue >= curShieldValue)
            {
                damageInfo.FinalDamageValue -= curShieldValue;
                buff.Over();
            }
            else
            {
                buff.Blackboard.SetLongValue(typeof(AddShieldsBuff), id, curShieldValue - damageInfo.FinalDamageValue);
                buff.Owner.AttributeSet.AddShield(-damageInfo.FinalDamageValue);
                damageInfo.FinalDamageValue = 0;
            }
        }

        protected override void OnOver(Buff buff, int id, List<int> parms)
        {
            long curShieldValue = buff.Blackboard.GetLongValue(typeof(AddShieldsBuff), id);
            buff.Owner.AttributeSet.AddShield(-curShieldValue);
            buff.Blackboard.SetLongValue(typeof(AddShieldsBuff), id, 0);
            EventAfterCalDamage eventAfterCalDamage = buff.Blackboard.GetReferenceValue<EventAfterCalDamage>(typeof(AddShieldsBuff), id);
            buff.Owner.EventAfterCalDamage -= eventAfterCalDamage;
        }
    }
}