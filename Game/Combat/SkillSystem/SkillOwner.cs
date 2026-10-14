using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class SkillOwner : MonoController, ISkillOwner, IResettable
    {
        [ShowInInspector] public List<SkillInfo> SkillInfos { get; private set; }
        [ShowInInspector] public LinkedList<SkillInfo> ReleaseQueue { get; private set; }
        public SkillInfo NextSkill => ReleaseQueue.Count > 0 ? ReleaseQueue.First.Value : null;
        public IEntity Entity { get; private set; }
        public SkillInfo ReleaseSkill { get; set; }
        Dictionary<int, float> _skillTimerDic;
        List<int> _skillTimerIDList;

        protected override void OnAwake()
        {
            SkillInfos = new List<SkillInfo>();
            ReleaseQueue = new LinkedList<SkillInfo>();
            Entity = GetComponent<Entity>();
            _skillTimerDic = new Dictionary<int, float>();
            _skillTimerIDList = new List<int>();
        }

        public void Excute(float deltaTime)
        {
            if (!Entity.Active) return;

            SkillCoolDown(deltaTime);
        }

        public void DoRelease(SkillInfo skillInfo)
        {
            if (!Entity.Active) return;
            if (skillInfo == null) return;
            if (skillInfo.Config == null) return;

            SetSkillTimer(skillInfo.Data.ID, GetSkillTimer(skillInfo.Data.ID) + skillInfo.Data.CD);
            if (skillInfo.Config.AnimationName == EAnimationName.none)
            {
                CreateSkill();
            }
            else
            {
                Entity.AnimationController.PlayAnimation(skillInfo.Config.AnimationName);
            }
        }

        public void CreateSkill(int id)
        {
            if (!Entity.Active) return;
            if (ReleaseSkill == null || ReleaseSkill.Data == null || ReleaseSkill.Data.ID != id) return;

            if (ReleaseSkill.TargetIsFriendly)
            {
                CreateSkill(ReleaseSkill, EntityInfo.Build(Entity));
            }
            else
            {
                CreateSkill(ReleaseSkill, Entity.AIAgent.Target);
            }
        }

        public void CreateSkill()
        {
            if (!Entity.Active) return;

            // 设置当前目标
            if (ReleaseSkill.TargetIsFriendly)
            {
                CreateSkill(ReleaseSkill, EntityInfo.Build(Entity));
            }
            else
            {
                CreateSkill(ReleaseSkill, Entity.AIAgent.Target);
            }
        }

        public void CreateSkill(SkillInfo skillInfo, EntityInfo lockTarget = default)
        {
            if (!Entity.Active) return;
            if (skillInfo == null) return;
            if (skillInfo.Config == null) return;

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(Entity.CombatType);

            combatManager.SkillManager.ExcuteSkill(skillInfo, lockTarget);
        }

        void SkillCoolDown(float deltaTime)
        {
            for (int i = 0; i < _skillTimerIDList.Count; i++)
            {
                if (_skillTimerDic[_skillTimerIDList[i]] <= 0) continue;

                _skillTimerDic[_skillTimerIDList[i]] -= deltaTime;
            }
        }

        public void SetSkillTimer(int skillID, float time)
        {
            if (!Entity.Active) return;

            if (!_skillTimerDic.ContainsKey(skillID))
            {
                _skillTimerDic.Add(skillID, time);
                _skillTimerIDList.Add(skillID);
            }
            else
            {
                _skillTimerDic[skillID] = time;
            }
        }

        public float GetSkillTimer(int skillID)
        {
            if (!Entity.Active) return 0;

            if (_skillTimerDic.ContainsKey(skillID))
            {
                return _skillTimerDic[skillID];
            }
            else
            {
                return 0;
            }
        }

        public void OnReset()
        {
            _skillTimerDic.Clear();
            _skillTimerIDList.Clear();
            RecycleSkillInfos();
        }

        void RecycleSkillInfos()
        {
            this.GetSystem<ISkillFactory>().RecycleSkillInfos(SkillInfos);

            SkillInfos.Clear();
            ReleaseQueue.Clear();
        }

#if UNITY_EDITOR
        [Button("释放自定义技能")]
        void ReleaseSkill_Test2(int skillID, SkillConfig config)
        {
            this.GetSystem<ICombatSystem>().GetCombatManager(Entity.CombatType).SkillManager.Clear();
            IAIAgent.StopAI = false;
            SkillInfo skillData = new SkillInfo();
            skillData.Data = DataTable.GetItem<DTSkill_base>(skillID);
            skillData.Owner = EntityInfo.Build(Entity);
            skillData.Config = config;
            this.GetSystem<ISkillFactory>().SetSkillLayer(skillData, Entity.IsPlayer);
            Entity.AIAgent.StopBehavior();
            Entity.AIAgent.StartBehavior();
            Entity.AnimationController.BindAnimationEvent(new List<SkillInfo> { skillData }, false);
            ReleaseQueue.Clear();
            ReleaseQueue.AddLast(skillData);
        }

        void OnDrawGizmosSelected()
        {
            if (ReleaseQueue == null || NextSkill == null) return;

            AreaDebug.DrawWireRound(transform.position, NextSkill.Config.SelectArea, Color.blue);
        }
#endif
    }
}