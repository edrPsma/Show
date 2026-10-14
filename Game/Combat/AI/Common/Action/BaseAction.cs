using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 行为基类
    /// </summary>
    public abstract class BaseAction : Action, IController
    {
        protected IEntity _entity;
        public IArchitecture GetArchitecture() => GameRoot.Interface;

        public override void OnAwake()
        {
            _entity = GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}