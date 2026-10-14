using System.Collections;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.GetHurt)]
    public class GetHurtBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            int percentage = parms[0];// 提升的万分比
            int type = parms[1];// 类型

            EventAfterCalDamage eventAfterCalDamage = (ref DamageInfo damageInfo, IEntity attacker, IEntity target, int skillID) =>
            {
                OnEventAfterCalDamage(ref damageInfo, target, buff, percentage, type);
            };
            buff.Blackboard.SetReferenceValue(typeof(SimpleAttackDamageBuff), id, eventAfterCalDamage);
            buff.Owner.EventAfterCalDamage += eventAfterCalDamage;
        }

        private void OnEventAfterCalDamage(ref DamageInfo damageInfo, IEntity target, Buff buff, int percentage, int type)
        {
            if (target != buff.Owner) return;

            if (type == 1)
            {
                float percent = percentage * buff.Multiple;
                long addValue = CombatUtility.CeilToLong(damageInfo.FinalDamageValue * percent * 0.0001d);
                damageInfo.FinalDamageValue += addValue;
            }
            else if (type == 2)
            {
                long addValue = CombatUtility.CeilToLong(damageInfo.FinalDamageValue * percentage * 0.0001d);
                damageInfo.FinalDamageValue += addValue;
            }
            else
            {
                Debug.LogError($"Combat 未定义的类型,Type:{type}");
            }
        }

        protected override void OnOver(Buff buff, int id, List<int> parms)
        {
            EventAfterCalDamage eventAfterCalDamage = buff.Blackboard.GetReferenceValue<EventAfterCalDamage>(typeof(SimpleAttackDamageBuff), id);
            buff.Owner.EventAfterCalDamage -= eventAfterCalDamage;
        }
    }
}