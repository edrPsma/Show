using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class BuffOwner : MonoController, IBuffOwner, IResettable
    {
        public IEntity Entity { get; private set; }

        [ShowInInspector] Dictionary<int, Buff> _buffDic;

        HashSet<DTSkill_buff.ETage> _ignoreList;
        Dictionary<string, int> _buffPrefabRefDic;// buff特效引用字典
        Dictionary<string, GameObject> _buffPrefabDic;
        List<int> _loadBuffPrefabTaskList;// buff特效加载列表

        protected override void OnAwake()
        {
            Entity = GetComponent<Entity>();
            _buffDic = new Dictionary<int, Buff>();
            _ignoreList = new HashSet<DTSkill_buff.ETage>();
            _buffPrefabRefDic = new Dictionary<string, int>();
            _buffPrefabDic = new Dictionary<string, GameObject>();
            _loadBuffPrefabTaskList = new List<int>();
        }

        public void Excute(float deltaTime)
        {
            if (_buffDic.Count <= 0) return;

            Buff[] buffs = _buffDic.Values.ToArray();
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i].Update(deltaTime);
            }
        }

        public void AddBuff(int buffId, EntityInfo caster, float multiple, int skillID, int skillLevel)
        {
            if (!Entity.Active) return;

            DTSkill_buff data = DataTable.GetItem<DTSkill_buff>(buffId);

            // 检测是否免疫该buff类型
            if (!_ignoreList.Contains(data.Tage))
            {
                IBuffFactory buffFactory = this.GetSystem<IBuffFactory>();
                Buff buff = buffFactory.Create(buffId, caster, Entity);
                buff.Multiple = multiple;
                buff.SkillID = skillID;
                buff.SkillLevel = skillLevel;

                // 如果重复添加 则用新的覆盖旧的
                if (_buffDic.ContainsKey(buffId))
                {
                    _buffDic[buffId].Over();
                }

                _buffDic.Add(buffId, buff);
                AddBuffPrefab(buffId);
                buff.Excute();
            }
        }

        public void AddToIgnoreList(DTSkill_buff.ETage tage)
        {
            if (!Entity.Active) return;

            _ignoreList.Add(tage);
        }

        public void RemoveFormIgnoreList(DTSkill_buff.ETage tage)
        {
            if (!Entity.Active) return;

            _ignoreList.Remove(tage);
        }

        public void RemoveBuff(Buff buff)
        {
            IBuffFactory buffFactory = this.GetSystem<IBuffFactory>();
            if (_buffDic.ContainsKey(buff.Data.ID))
            {
                RemoveBuffPrefab(buff.Data.ID);
                _buffDic.Remove(buff.Data.ID);
                buffFactory.ReleaseBuff(buff);
            }
        }

        void RemoveAllBuff()
        {
            Buff[] buffs = _buffDic.Values.ToArray();
            foreach (var item in buffs)
            {
                RemoveBuff(item);
            }
            _buffDic.Clear();
        }

        public void Disperse(int buffID)
        {
            _buffDic.GetValue(buffID)?.Disperse();
        }

        public int[] GetAllBuff()
        {
            return _buffDic.Keys.ToArray();
        }

        public Buff GetBuff(int buffID)
        {
            if (_buffDic.ContainsKey(buffID))
            {
                return _buffDic[buffID];
            }

            return null;
        }

        public void OnReset()
        {
            RemoveAllBuff();
            _ignoreList.Clear();
            ClearAllBuffEffect();
        }

        void AddBuffPrefab(int buffId)
        {
            var data = DataTable.GetItem<DTSkill_buff>(buffId);
            if (string.IsNullOrEmpty(data.Effect)) return;

            if (!_buffPrefabRefDic.ContainsKey(data.Effect))
            {
                _buffPrefabRefDic.Add(data.Effect, 1);
                int taskID = 0;
                taskID = GameEntry.Prefab.Instantiate(data.Effect, prefab =>
                {
                    _loadBuffPrefabTaskList.Remove(taskID);
                    if (!_buffPrefabRefDic.ContainsKey(data.Effect))
                    {
                        GameEntry.Prefab.Release(prefab, false);
                    }
                    else
                    {
                        if (!_buffPrefabDic.ContainsKey(data.Effect))
                        {
                            _buffPrefabDic.Add(data.Effect, prefab);
                            switch (data.Effect_position)
                            {
                                case DTSkill_buff.EEffect_position.Tou_bu: prefab.SetParent(Entity.AnimationController.HeadTrans); break;
                                case DTSkill_buff.EEffect_position.Shen_ti: prefab.SetParent(Entity.AnimationController.BodyTrans); break;
                                case DTSkill_buff.EEffect_position.Jiao_di: prefab.SetParent(Entity.GameObject.transform); break;
                                case DTSkill_buff.EEffect_position.Gen_sui_zuo_qi_jiao_di: prefab.SetParent(Entity.AnimationController.ModelTrans); break;
                            }
                        }
                        else
                        {
                            GameEntry.Prefab.Release(prefab, false);
                            Debug.LogError($"Combat buff特效重复添加, BuffID: {buffId}");
                        }
                    }
                }, null, EPrefabTag.Combat);
                _loadBuffPrefabTaskList.Add(taskID);
            }
            else
            {
                _buffPrefabRefDic[data.Effect]++;
            }
        }

        void RemoveBuffPrefab(int buffId)
        {
            var data = DataTable.GetItem<DTSkill_buff>(buffId);
            if (string.IsNullOrEmpty(data.Effect)) return;

            if (_buffPrefabRefDic.ContainsKey(data.Effect))
            {
                _buffPrefabRefDic[data.Effect]--;
                if (_buffPrefabRefDic[data.Effect] <= 0)
                {
                    _buffPrefabRefDic.Remove(data.Effect);
                    if (_buffPrefabDic.ContainsKey(data.Effect))
                    {
                        GameEntry.Prefab.Release(_buffPrefabDic[data.Effect], false);
                        _buffPrefabDic.Remove(data.Effect);
                    }
                    // else if (!_buffPrefabDic.ContainsKey(data.Effect) && !Entity.AIAgent.IsDeath)
                    // {
                    //     Debug.LogError($"Combat buff特效还未加载完成就删除了, BuffID: {buffId}");
                    // }
                }
            }
            else
            {
                Debug.LogError("Combat buff特效计数异常,BuffID: {buffId}");
            }
        }

        void ClearAllBuffEffect()
        {
            for (int i = 0; i < _loadBuffPrefabTaskList.Count; i++)
            {
                int taskID = _loadBuffPrefabTaskList[i];
                GameEntry.Prefab.Cancel(ref taskID);
            }
            _loadBuffPrefabTaskList.Clear();
            foreach (var item in _buffPrefabDic)
            {
                GameEntry.Prefab.Release(item.Value, false);
            }
            _buffPrefabDic.Clear();
            _buffPrefabRefDic.Clear();
        }
    }
}