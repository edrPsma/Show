using System;
using System.Collections;
using System.Collections.Generic;
using Ev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public class AttributeItem : IAttribute
    {
        [ShowInInspector, ReadOnly] public EAttributeKey Key { get; }
        [ShowInInspector] public long Max { get; set; } = int.MaxValue;
        [ShowInInspector] public long Min { get; set; } = int.MinValue;
        [ShowInInspector]
        public long Value
        {
            get => _value.Value;
            set => _value.Value = CombatUtility.Clamp(value, Min, Max);
        }
        public bool CanModifly { get; set; }

        private BindProperty<long> _value = new BindProperty<long>(0);

        public void Subscribe(Action<long> onValueChange, bool runInFirst = true)
        {
            _value.Subscribe(onValueChange, runInFirst);
        }

        public void Unsubscribe(Action<long> onValueChange)
        {
            _value.Unsubscribe(onValueChange);
        }

        public void UnsubscribeAll()
        {
            _value.UnsubscribeAll();
        }
    }
}

