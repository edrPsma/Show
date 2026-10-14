using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class BaseEffector : Controller, ISkillEffector
    {
        void ISkillEffector.Excute(SkillExcutor excutor, EntityInfo skillOwner, float deltaTime, List<int> param)
        {
            OnExcute(excutor, skillOwner, deltaTime, param);
        }

        void IReleaseSkillEffector.TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {
            OnTakeReleaseSkillEffect(excutor, skillOwner, param);
        }

        void ITakeDamageBeforeEffector.TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            OnTakeDamageBeforeEffect(excutor, skillOwner, target, ref damageInfo, param);
        }

        void ITakeDamageAfterEffector.TakeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {
            OnTakeDamageAfterEffect(excutor, skillOwner, target, ref damageInfo, param);
        }

        protected virtual void OnTakeReleaseSkillEffect(SkillExcutor excutor, EntityInfo skillOwner, List<int> param)
        {

        }

        protected virtual void OnTakeDamageBeforeEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {

        }

        protected virtual void OnTakeDamageAfterEffect(SkillExcutor excutor, EntityInfo skillOwner, EntityInfo target, ref DamageInfo damageInfo, List<int> param)
        {

        }

        protected virtual void OnExcute(SkillExcutor excutor, EntityInfo skillOwner, float deltaTime, List<int> param)
        {

        }
    }
}