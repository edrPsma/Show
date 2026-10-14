using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Disperse)]
    public class DisperseBuffEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Disperse(target, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Disperse(target, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Disperse(excutor.LockTarget, param);
        }

        void Disperse(EntityInfo target, List<int> param)
        {
            int num = param[0];// 驱散的数量

            int count = num;
            int[] buffs = target.Entity.BuffOwner.GetAllBuff();
            for (int j = 0; j < buffs.Length; j++)
            {
                var data = DataTable.GetItem<DTSkill_buff>(buffs[j]);
                if (data.Tage == DTSkill_buff.ETage.D_e_b_u_f_f)
                {
                    target.Entity.BuffOwner.Disperse(buffs[j]);
                    count--;
                }
                if (count <= 0) break;
            }
        }
    }
}