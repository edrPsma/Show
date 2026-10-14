using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Combat
{
    public abstract class EntityBuilder<TData> : Controller, IEntityBuilder where TData : BaseData<TData>, new()
    {
        protected IEntityFactory _factory;
        public void Build(IEntityFactory factory, Entity entity, int entityId, bool isPlayer, BuildInfo userData)
        {
            _factory = factory;
            entity.EntityID = entityId;
            entity.IsPlayer = isPlayer;
            entity.SetDirection(isPlayer ? 1 : -1);
            var data = DataTable.GetItem<TData>(entityId);
            BindEvent(entity);
            SetParameter(entity, data, isPlayer, userData);
            LoadModel(entity, data);
            entity.AIAgent.Init(GetAIBehaviour());
            SetLayer(entity, isPlayer);
        }

        /// <summary>
        /// 获取模型位置
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        protected abstract string GetModelPath(TData data);

        /// <summary>
        /// 获取AI Set
        /// </summary>
        /// <returns></returns>
        protected abstract ExternalBehavior GetAIBehaviour();

        /// <summary>
        /// 绑定事件
        /// </summary>
        protected abstract void BindEvent(IEntity entity);

        /// <summary>
        /// 设置参数
        /// </summary>
        protected abstract void SetParameter(Entity entity, TData data, bool isPlayer, BuildInfo buildInfo);

        void LoadModel(Entity entity, TData data)
        {
            entity.AnimationController.LoadModel(GetModelPath(data));
        }

        void SetLayer(Entity entity, bool isPlayer)
        {
            if (isPlayer)
            {
                entity.HitBox.gameObject.layer = LayerDefine.PLAYER;
            }
            else
            {
                entity.HitBox.gameObject.layer = LayerDefine.MONSTER;
            }
        }
    }
}