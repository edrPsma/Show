using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using Sirenix.OdinInspector;
using Template;
using UnityEngine;
using UnityEngine.Animations;

namespace Combat
{
    [System(typeof(IEntityFactory))]
    public class EntityFactory : AbstractSystem, IEntityFactory
    {
        [ShowInInspector] GameObjectPool<Entity> _entityPool;
        public Transform SceneTrans { get; private set; }
        Transform _recycleTrans;
        int _entityGuid = 1;
        bool _dataLoadOver;

        protected override void OnLoadData()
        {
            if (_dataLoadOver)
            {
                LoadDataDone();
                return;
            }
            InitEntityPool();
        }

        public IEntity Create(ECombatType combatType, EEntityType entityType, int entityId, bool isPlayer, BuildInfo buildInfo = default)
        {

            IEntityBuilder builder = GameEntry.Reflection.Get<IEntityBuilder>(entityType);
            if (builder != null)
            {
                Entity entity = _entityPool.SpawnByType();
                entity.CombatType = combatType;
                entity.GUID = _entityGuid++;
                entity.EntityType = entityType;
                if (entity.GUID == 0)
                {
                    entity.GUID = ++_entityGuid;
                }
                builder.Build(this, entity, entityId, isPlayer, buildInfo);

                Debug.Log($"Combat 生成单位 CombatType:{combatType} GUID:{entity.GUID} EneityID:{entityId} IsPlayer:{isPlayer}");

                entity.Init();
                return entity;
            }
            else
            {
                Debug.LogError($"Combat 没有此类型的单位构建器, Type:{entityType}");
                return null;
            }
        }

        public IEntity CreateEmpty()
        {
            return _entityPool.SpawnByType();
        }

        public void Release(IEntity entity)
        {
            _entityPool.Release(entity);
        }

        public void SetSceneTrans(Transform movePoint)
        {
            PositionFollower positionConstraint = SceneTrans.GetComponent<PositionFollower>();
            positionConstraint.SetTarget(movePoint);
        }

        #region Init
        void InitEntityPool()
        {
            GameObject root = new GameObject("@EntityPool").ResetTransform();
            SceneTrans = new GameObject("SceneTrans").SetParent(root).transform;
            _recycleTrans = new GameObject("RecycleTrans").SetParent(root).transform;
            PositionFollower positionConstraint = SceneTrans.gameObject.AddComponent<PositionFollower>();

            GameObject.DontDestroyOnLoad(root);
            GameEntry.Resource.Load<GameObject>(root, "prefab/Combat/Entity", clone =>
            {
                Entity entity = clone.GetComponent<Entity>();
                _entityPool = new GameObjectPool<Entity>(EPoolType.Scalable, 20, entity, entity => entity.SetParent(_recycleTrans), null);
                _entityPool.OnReleaseEvent += OnEntityRecycle;
                _entityPool.OnGetEvent += OnEntityGet;

                _recycleTrans.SetActive(false);
                _dataLoadOver = true;
                LoadDataDone();
            });
        }

        #endregion

        private void OnEntityGet(Entity entity)
        {
            entity.SetParent(SceneTrans);
        }

        private void OnEntityRecycle(Entity entity)
        {
            if (entity == null) return;

            IResettable[] resettables = entity.GetComponentsInChildren<IResettable>(true);
            for (int i = 0; i < resettables.Length; i++)
            {
                resettables[i].OnReset();
            }
            Debug.Log($"Combat 回收 GUID: {entity.GUID} ID: {entity.EntityID}");
            entity.GUID = 0;
            entity.SetParent(_recycleTrans, true);
        }
    }

}
