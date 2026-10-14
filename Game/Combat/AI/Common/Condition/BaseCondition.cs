using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 条件基类
    /// </summary>
    public abstract class BaseCondition : Conditional
    {
        protected IEntity _entity;

        public override void OnAwake()
        {
            _entity = GetComponent<Entity>();
        }
    }
}