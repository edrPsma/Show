using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public readonly struct EntityInfo
    {
        [ShowInInspector] public int GUID { get; }
        [ShowInInspector] public IEntity Entity { get; }

        [ShowInInspector] public bool IsValid => Entity != null && GUID == Entity.GUID;
        public Vector3 Position => Entity.GameObject.transform.position;
        public Vector3 StartPosition { get; }
        public Vector3 StartBodyPosition { get; }
        public Vector3 StartHeadPosition { get; }
        [ShowInInspector] public bool IsAlive => IsValid && Entity.AttributeSet.HP > 0 && !Entity.AIAgent.IsDeath;

        private EntityInfo(IEntity entity)
        {
            GUID = entity != null ? entity.GUID : 0;
            Entity = entity;
            StartPosition = entity.GameObject.transform.position;
            StartBodyPosition = entity.AnimationController.BodyTrans.position;
            StartHeadPosition = entity.AnimationController.HeadTrans.position;
        }

        public override bool Equals(object obj)
        {
            if (obj is EntityInfo)
            {
                return ((EntityInfo)obj).GUID == GUID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return GUID.GetHashCode();
        }

        public static EntityInfo Build(IEntity entity)
        {
            return new EntityInfo(entity);
        }
    }
}