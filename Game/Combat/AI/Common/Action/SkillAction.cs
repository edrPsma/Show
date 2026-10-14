using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 释放技能行为
    /// </summary>
    public class SkillAction : BaseAction
    {
        IUnRegister _unRegister;
        bool _releaseOver;
        public override void OnStart()
        {
            _releaseOver = false;

            ExcuteSkill();
            _unRegister = _entity.EventSource.Register<int>(EntityEvent.SkillActionDone, skillID => OnSkillActionDone(skillID));
        }

        public override TaskStatus OnUpdate()
        {
            if (_entity.SkillOwner.ReleaseSkill == null) return TaskStatus.Failure;

            if (_releaseOver)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Running;
            }
        }

        public override void OnEnd()
        {
            _unRegister?.UnRegister();
            _unRegister = null;
            _entity.AnimationController.PlayAnimation(EAnimationName.idle);
            _entity.AIAgent.SetTarget(null);
        }

        public override void OnConditionalAbort()
        {
            _unRegister?.UnRegister();
            _unRegister = null;
            _entity.AnimationController.PlayAnimation(EAnimationName.idle);
            _entity.AIAgent.SetTarget(null);
        }

        private void OnSkillActionDone(int skillID)
        {
            if (skillID != _entity.SkillOwner.ReleaseSkill.Data.ID)
            {
                // Debug.LogError($"单位技能可能释放异常,GUID:{_entity.GUID} SKillID:{skillID}");
                return;
            }

            _releaseOver = true;
            if (_entity.SkillOwner.ReleaseQueue.Count > 0)
            {
                _entity.SkillOwner.ReleaseQueue.RemoveFirst();
            }
        }

        private void ExcuteSkill()
        {
            _releaseOver = false;
            _entity.SkillOwner.ReleaseSkill = _entity.SkillOwner.NextSkill;
            _entity.EventSource.TriggerNow(EntityEvent.BeforeAttackWithAction, _entity);
            _entity.SkillOwner.DoRelease(_entity.SkillOwner.ReleaseSkill);
            if (_entity.SkillOwner.ReleaseSkill.Config.AnimationName == EAnimationName.none)
            {
                _releaseOver = true;
                _entity.SkillOwner.ReleaseQueue.RemoveFirst();
            }
        }
    }
}