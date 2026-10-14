using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using Skill;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    [System(typeof(ISkillFactory))]
    public class SkillFactory : AbstractSystem, ISkillFactory
    {
        const string CONFIG_PATH = "so/SkillConfig/{0}";
        ObjectPool<SkillInfo> _skillPool;
        ObjectPool<EffectorItem> _effectorItemPool;
        Dictionary<string, SkillConfig> _skillConfig;
        bool _dataLoadOver;
        List<SkillInfo> _waitToRecycleList;


        protected override void OnInit()
        {
            _waitToRecycleList = new List<SkillInfo>();
            _skillPool = GameEntry.Pool.GetOrCreatePool<SkillInfo>(EPoolType.Scalable, 50) as ObjectPool<SkillInfo>;
            _skillPool.OnReleaseEvent += skillInfo => skillInfo.OnReset();
            _effectorItemPool = GameEntry.Pool.GetOrCreatePool<EffectorItem>(EPoolType.Scalable, 50) as ObjectPool<EffectorItem>;
            _effectorItemPool.OnReleaseEvent += item => item.OnReset();
            GameEntry.Task.AddTask(t => Update(t.Times)).SetRepeatTimes(-1).SetName("SkillFactory.Update").Run();
        }

        protected override void OnLoadData()
        {
            if (_dataLoadOver)
            {
                LoadDataDone();
                return;
            }
            LoadSkillData();
        }

        #region LoadSkillConfig
        void LoadSkillData()
        {
            _skillConfig = new Dictionary<string, SkillConfig>();

            int configCount = 0;
            int counter = 0;
            var datas = DataTable.GetArray<DTSkill_base>();
            HashSet<string> loadList = new HashSet<string>();
            foreach (var item in datas)
            {
                if (loadList.Contains(item.File)) continue;

                loadList.Add(item.File);
                configCount++;
                GameEntry.Resource.Load<SkillConfig>(GameEntry.Prefab.RecycleNode, string.Format(CONFIG_PATH, item.File), config =>
                {
                    if (config != null)
                    {
                        if (!_skillConfig.ContainsKey(item.File))
                        {
                            _skillConfig.Add(item.File, config);
                        }
                    }
                    counter++;
                    if (counter == configCount)
                    {
                        LoadDataDone();
                        _dataLoadOver = true;
                    }
                });
            }
        }
        #endregion

        private void Update(uint obj)
        {
            CheckRecycleSkillInfo();
        }

        void CheckRecycleSkillInfo()
        {
            for (int i = _waitToRecycleList.Count - 1; i >= 0; i--)
            {
                if (_waitToRecycleList[i].RefCount == 0)
                {
                    _skillPool.Release(_waitToRecycleList[i]);
                    _waitToRecycleList.Remove(_waitToRecycleList[i]);
                }
                else if (_waitToRecycleList[i].RefCount < 0)
                {
                    _skillPool.Release(_waitToRecycleList[i]);
                    _waitToRecycleList.Remove(_waitToRecycleList[i]);
                    Debug.LogError("Combat 技能回收有问题");
                }
            }
        }

        public ISelector GetSelector(ESelectType type)
        {
            return GameEntry.Reflection.Get<ISelector>(type);
        }

        public Type GetSkillExcutorType(SkillInfo skillInfo)
        {
            if (skillInfo == null || skillInfo.Data == null)
            {
                Debug.LogError("Combat 技能信息为空");
                return null;
            }
            else if (skillInfo.Config == null)
            {
                Debug.LogError($"Combat 技能配置文件为空,技能ID: {skillInfo.Data.ID}");
                return null;
            }
            return GameEntry.Reflection.Get<SkillExcutor, Type>(skillInfo.Config.RuleType);
        }

        public void RecycleSkillInfo(SkillInfo skillInfo)
        {
            _waitToRecycleList.Add(skillInfo);
        }

        public void RecycleSkillInfos(List<SkillInfo> skillInfos)
        {
            for (int i = 0; i < skillInfos.Count; i++)
            {
                _waitToRecycleList.Add(skillInfos[i]);
            }
        }

        public void BindSkill(Entity entity, int[] skillIds)
        {
            for (int i = 0; i < skillIds.Length; i++)
            {
                SkillInfo skillInfo = SpawnSkillInfo(EntityInfo.Build(entity), skillIds[i]);
                entity.SkillOwner.SkillInfos.Add(skillInfo);
            }
        }

        public SkillInfo SpawnSkillInfo(EntityInfo entityInfo, int skillId)
        {
            if (!entityInfo.IsValid) return null;

            SkillInfo skillInfo = _skillPool.SpawnByType();
            skillInfo.Data = DataTable.GetItem<DTSkill_base>(skillId);
            skillInfo.Config = _skillConfig.GetValue(skillInfo.Data.File);

            if (skillInfo.Config == null)
            {
                Debug.LogError($"Combat 配置文件为空,技能ID: {skillId}");
            }

            skillInfo.Owner = entityInfo;
            SetSkillLayer(skillInfo, skillInfo.Owner.Entity.IsPlayer);

            return skillInfo;
        }

        public SkillConfig GetSkillConfig(int skillId)
        {
            var data = DataTable.GetItem<DTSkill_base>(skillId);

            return _skillConfig.GetValue(data.File);
        }

        public void SetSkillLayer(SkillInfo skillInfo, bool isPlayer)
        {
            if (isPlayer)
            {
                if (skillInfo.Data.Target == ETarget.You_jun)
                {
                    skillInfo.LayerMask = LayerDefine.PLAYER_LAYERMASK;
                    skillInfo.TargetIsPlayer = true;
                }
                else
                {
                    skillInfo.LayerMask = LayerDefine.MONSTER_LAYERMASK;
                    skillInfo.TargetIsPlayer = false;
                }
            }
            else
            {
                if (skillInfo.Data.Target == ETarget.You_jun)
                {
                    skillInfo.LayerMask = LayerDefine.MONSTER_LAYERMASK;
                    skillInfo.TargetIsPlayer = false;
                }
                else
                {
                    skillInfo.LayerMask = LayerDefine.PLAYER_LAYERMASK;
                    skillInfo.TargetIsPlayer = true;
                }
            }
        }

        public void GetSkillEffector(SkillInfo skillInfo, List<EffectorItem> effectors)
        {
            effectors.Add(GetImpactorDefult());
            int index = CalEffectIndex(skillInfo);
            if (skillInfo.Data.Effect2 != null)
            {
                for (int i = 0; i < skillInfo.Data.Effect2[index].Count; i++)
                {
                    effectors.Add(GetSkillEffector(skillInfo.Data.Effect2[index][i]));
                }
            }

        }

        int CalEffectIndex(SkillInfo skillInfo)
        {
            SkillData skillData = this.GetModel<ISkillModel>().GetSkillDataFromSkillId(skillInfo.Data.ID);
            int level = skillData == null ? 1 : skillData.level;
            int index = 0;
            if (skillInfo.Data.Effect1 == null) return 0;

            for (int i = 0; i < skillInfo.Data.Effect1.Count; i++)
            {
                if (skillInfo.Data.Effect1[i] <= level)
                {
                    index = i;
                }
                else
                {
                    break;
                }
            }

            return index;
        }

        EffectorItem GetImpactorDefult()
        {
            EffectorItem effectorItem = _effectorItemPool.SpawnByType();
            effectorItem.EffectId = 0;
            effectorItem.Effector = GameEntry.Reflection.Get<ISkillEffector>(EEffectType.Damage);
            effectorItem.TriggerTiming = DTSkill_effect.ETime.Gong_ji_hou;
            return effectorItem;
        }

        public EffectorItem GetSkillEffector(int id)
        {
            var data = DataTable.GetItem<DTSkill_effect>(id);
            if (data == null)
            {
                Debug.LogError($"Combat 不存在该效果器,ID: {id}");
                return null;
            }
            else
            {
                EffectorItem impactor = _effectorItemPool.SpawnByType();
                impactor.EffectId = id;
                impactor.Effector = GameEntry.Reflection.Get<ISkillEffector>((EEffectType)data.Type);
                if (impactor.Effector == null)
                {
                    Debug.LogError($"Combat 效果器不存在,Type: {(EEffectType)data.Type}");
                }
                impactor.TriggerTiming = data.Time;
                impactor.EffectValue = data.EffectValue;
                return impactor;
            }
        }

        public void RecycleEffectorItems(List<EffectorItem> effectors)
        {
            foreach (var item in effectors)
            {
                _effectorItemPool.Release(item);
            }

            effectors.Clear();
        }
    }
}