using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.ReduceCd)]
    public class ReduceCdEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            ReduceCd(excutor, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            ReduceCd(excutor, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            ReduceCd(excutor, param);
        }

        void ReduceCd(SkillExcutor excutor, List<int> param)
        {
            int second = param[0];
            List<SkillInfo> skillList = excutor.SkillInfo.Owner.Entity.SkillOwner.SkillInfos;
            for (int i = 0; i < skillList.Count; i++)
            {
                float curTimer = excutor.SkillInfo.Owner.Entity.SkillOwner.GetSkillTimer(skillList[i].Data.ID);
                excutor.SkillInfo.Owner.Entity.SkillOwner.SetSkillTimer(skillList[i].Data.ID, Mathf.Clamp(curTimer - second, curTimer, curTimer));
            }
        }
    }
}