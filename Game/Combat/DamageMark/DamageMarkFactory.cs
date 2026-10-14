using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

namespace Combat
{
    public interface IDamageMarkFactory : ISystem
    {

        /// <summary>
        /// 回收伤害标记
        /// </summary>
        /// <param name="damageMark"></param>
        void Release(DamageMark damageMark);

        /// <summary>
        /// 显示伤害标记
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        /// <param name="damageInfo"></param>
        void Show(Vector3 pos, float dir, ref DamageInfo damageInfo);
    }

    [System(typeof(IDamageMarkFactory))]
    public class DamageMarkFactory : AbstractSystem, IDamageMarkFactory
    {
        GameObjectPool<DamageMark> _pool;
        bool _loadOver;
        protected override void OnLoadData()
        {
            if (_loadOver)
            {
                LoadDataDone();
                return;
            }

            InitPool();
        }

        void InitPool()
        {
            GameObject recycleNode = new GameObject(nameof(DamageMarkFactory));
            GameObject.DontDestroyOnLoad(recycleNode);
            recycleNode.SetActive(false);
            GameEntry.Resource.Load<GameObject>(GameEntry.Prefab.RecycleNode, "prefab/Combat/DamageMark", prefab =>
            {
                DamageMark damageMark = prefab.GetComponent<DamageMark>();
                _pool = new GameObjectPool<DamageMark>(EPoolType.Scalable, 30, damageMark,
                clone => clone.SetParent(recycleNode), null);

                _pool.OnReleaseEvent += clone => clone.SetParent(recycleNode);
                _pool.OnGetEvent += clone => clone.transform.SetParent(null);
                _loadOver = true;
                LoadDataDone();
            });
        }

        public void Show(Vector3 pos, float dir, ref DamageInfo damageInfo)
        {
            _pool.SpawnByType().Show(pos, dir, ref damageInfo);
        }

        public void Release(DamageMark damageMark)
        {
            _pool.Release(damageMark);
        }
    }
}