using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Effector(EEffectType.Beheaded)]
    public class BeheadedEffector : BaseEffector
    {
        protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Beheaded(target, ref damageInfo, param);
        }

        protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            Beheaded(target, ref damageInfo, param);
        }

        protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            Debug.LogError($"Combat 斩杀效果配置时机不正确");
        }

        void Beheaded(EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            int hpPercentage = param[0];// 当前血量百分比
            var hp = target.Entity.AttributeSet.HPAttribute;
            float nowPercentage = hp.Value * 1f / hp.Max;
            if (hpPercentage * 0.01f <= nowPercentage)
            {
                damageInfo.IsValid = true;
                damageInfo.DamageValue = hp.Value;
                damageInfo.CriticalDamageValue = hp.Value;
                damageInfo.FinalDamageValue = hp.Value;
            }
        }
    }
}