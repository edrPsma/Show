using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class SummonManager : MonoController, IResettable
    {
        SummonPosInfo[] _posInfos;
        protected override void OnAwake()
        {
            Init();
        }

        void Init()
        {
            Transform[] transforms = transform.GetComponentsInChildren<Transform>();
            _posInfos = new SummonPosInfo[transforms.Length - 1];
            for (int i = 1; i < transforms.Length; i++)
            {
                _posInfos[i - 1] = new SummonPosInfo
                {
                    Transform = transforms[i],
                    Entity = null,
                };
            }
        }

        public void AddSummon(IEntity entity)
        {
            if (entity == null) return;

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);

            for (int i = 0; i < _posInfos.Length; i++)
            {
                if (_posInfos[i].Entity == null)
                {
                    _posInfos[i].Entity = entity;
                    entity.GameObject.SetParent(_posInfos[i].Transform);
                    entity.GameObject.transform.localPosition = Vector3.zero;
                    combatManager.AddOther(entity);
                    return;
                }
            }

            combatManager.RemoveOther(_posInfos[0].Entity);
            _posInfos[0].Entity = entity;
            entity.GameObject.SetParent(_posInfos[0].Transform);
            combatManager.AddOther(entity);
        }

        public void RemoveSummon(IEntity entity)
        {
            if (entity == null) return;

            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);
            for (int i = 0; i < _posInfos.Length; i++)
            {
                if (_posInfos[i].Entity == entity)
                {
                    combatManager.RemoveOther(entity);
                    _posInfos[i].Entity = null;
                    break;
                }
            }
        }

        public List<IEntity> GetAllEntity()
        {
            List<IEntity> result = new List<IEntity>();
            foreach (var item in _posInfos)
            {
                if (item.Entity != null)
                {
                    result.Add(item.Entity);
                }
            }

            return result;
        }

        public void Clear()
        {
            for (int i = 0; i < _posInfos.Length; i++)
            {
                if (_posInfos[i].Entity == null) continue;

                CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(_posInfos[i].Entity.CombatType);
                combatManager.RemoveOther(_posInfos[i].Entity);
                _posInfos[i].Entity = null;
            }
        }

        public void OnReset()
        {
            Clear();
        }
    }

    public class SummonPosInfo
    {
        public Transform Transform;
        public IEntity Entity;
    }
}