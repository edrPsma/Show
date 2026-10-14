using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class SkillManager
    {
        [ShowInInspector] Dictionary<int, Queue<ISkillExcutor>> _skillPool;// 技能对象池
        List<ISkillExcutor> _excuteList;// 当前正在执行的技能
        List<ISkillExcutor> _removeList;// 待移除的技能
        ECombatType _combatType;

        public SkillManager(ECombatType combatType)
        {
            _combatType = combatType;
            _skillPool = new Dictionary<int, Queue<ISkillExcutor>>();
            _excuteList = new List<ISkillExcutor>();
            _removeList = new List<ISkillExcutor>();
        }

        public void Clear()
        {
            _removeList.Clear();
            for (int i = 0; i < _excuteList.Count; i++)
            {
                if (_excuteList[i].Active)
                {
                    _excuteList[i].Dispose();
                }
            }
            _excuteList.Clear();
            _skillPool.Clear();
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _excuteList.Count; i++)
            {
                if (!_excuteList[i].Active)
                {
                    _removeList.Add(_excuteList[i]);
                }
                else
                {
                    _excuteList[i].Update(deltaTime);
                }
            }

            for (int i = 0; i < _removeList.Count; i++)
            {
                _excuteList.Remove(_removeList[i]);
            }
            _removeList.Clear();
        }

        public void ExcuteSkill(SkillInfo skillInfo, EntityInfo lockTarget)
        {
            // if (!lockTarget.IsValid) return;

            IEntity entity = skillInfo.Owner.Entity;

            ISkillExcutor skillExcutor = SpawnSkill(skillInfo);

            if (skillExcutor == null) return;

            // 若为自身位置
            if (skillInfo.Config.Bullet_place == EBulletPlace.Self)
            {
                Vector3 offset = new Vector3(skillInfo.Config.SkillPrefabModify.x * entity.Direction, skillInfo.Config.SkillPrefabModify.y, 0);

                // 普通攻击要适配坐骑
                if (skillInfo.Config.FollowMount)
                {
                    skillExcutor.SetPosition(entity.AnimationController.Transform.position + offset);
                }
                else
                {
                    skillExcutor.SetPosition(entity.GameObject.transform.position + offset);
                }
            }
            else
            {
                Vector3 offset = new Vector3(skillInfo.Config.SkillPrefabModify.x * entity.Direction, skillInfo.Config.SkillPrefabModify.y, 0);
                if (entity.AIAgent.Target.IsValid)
                {
                    skillExcutor.SetPosition(entity.AIAgent.Target.Position + offset);
                }
                else
                {
                    skillExcutor.SetPosition(entity.GameObject.transform.position + offset);
                }
            }
            float scale = skillInfo.Config.SkillPrefabModify.z;
            skillExcutor.SetScale(new Vector3(entity.Direction * scale, scale, scale));
            skillExcutor.Start(_combatType, lockTarget, skillInfo);

            _excuteList.Add(skillExcutor);
        }

        ISkillExcutor SpawnSkill(SkillInfo skillInfo)
        {
            if (!CheckIsValid(skillInfo)) return null;

            if (!_skillPool.ContainsKey(skillInfo.Data.ID))
            {
                _skillPool.Add(skillInfo.Data.ID, new Queue<ISkillExcutor>());
            }

            if (_skillPool[skillInfo.Data.ID].Count > 0)
            {
                return _skillPool[skillInfo.Data.ID].Dequeue();
            }
            else
            {
                ISkillExcutor skillExcutor = GameEntry.Reflection.Get<ISkillExcutor>(skillInfo.Config.RuleType).New();
                if (skillExcutor == null)
                {
                    Debug.LogError($"Combat 技能规则未定义 {skillInfo.Config.RuleType}");
                }
                return skillExcutor;
            }
        }

        public void RecycleSkill(ISkillExcutor skillExcutor, int skillID)
        {
            if (!_skillPool.ContainsKey(skillID))
            {
                _skillPool.Add(skillID, new Queue<ISkillExcutor>());
            }

            _skillPool[skillID].Enqueue(skillExcutor);
        }

        bool CheckIsValid(SkillInfo skillInfo)
        {
            if (skillInfo == null)
            {
                Debug.LogError("Combat 技能信息为空");
                return false;
            }

            if (skillInfo.Data == null)
            {
                Debug.LogError("Combat 技能表配置为空");
                return false;
            }

            if (skillInfo.Config == null)
            {
                Debug.LogError("Combat 技能配置文件为空");
                return false;
            }

            return true;
        }
    }
}
