using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class AIAgent : MonoController, IAIAgent, IResettable
    {
        public bool IsDeath => Entity.AttributeSet.HP <= 0;
        [ShowInInspector] public EntityInfo Target { get; private set; }
        public BehaviorTree BehaviorTree { get; private set; }
        public IEntity Entity { get; private set; }
        [ShowInInspector] public ClampInt CantDoAction { get; set; } = new ClampInt(0, 0, int.MaxValue);
        [ShowInInspector] public ClampInt CantMove { get; set; } = new ClampInt(0, 0, int.MaxValue);
        public bool Active => BehaviorTree == null ? false : _aiEnable;
        bool _aiEnable;

        protected override void OnAwake()
        {
            BehaviorTree = GetComponent<BehaviorTree>();
            Entity = GetComponent<Entity>();
        }

        public void Init(ExternalBehavior behavior)
        {
            StopBehavior();
            BehaviorTree.ExternalBehavior = behavior;
        }

        public void SetTarget(IEntity entity)
        {
            if (entity == null) return;

            Target = EntityInfo.Build(entity);
        }

        public void StartBehavior()
        {
            if (BehaviorTree == null) return;

            BehaviorTree.EnableBehavior();
            _aiEnable = true;
        }

        public void StopBehavior()
        {
            BehaviorTree?.DisableBehavior();
            _aiEnable = false;
        }

        public void OnReset()
        {
            StopBehavior();
            Target = default;
            BehaviorTree.ExternalBehavior = null;
            CantDoAction = CantDoAction.SetValue(0);
            CantMove = CantMove.SetValue(0);
        }
    }
}