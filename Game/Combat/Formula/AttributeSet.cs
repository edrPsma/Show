using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class AttributeSet : MonoBehaviour, IAttributeSet, IResettable
    {
        public const int HP_ID = 0;// 当前血量key值
        public const int Shield_ID = 2;// 当前护盾key值
        public long HP => GetValue(HP_ID);
        public IAttribute HPAttribute => GetAttribute(HP_ID);
        public IAttribute ShieldAttribute => GetAttribute(Shield_ID);
        [ShowInInspector] Dictionary<int, IAttribute> _dic;
        IEntity _entity;

        private void Awake()
        {
            _entity = GetComponent<IEntity>();
            _dic = new Dictionary<int, IAttribute>();
            foreach (var key in Enum.GetValues(typeof(EAttributeKey)))
            {
                _dic.Add((int)key, new AttributeItem());
            }
            _dic.Add(HP_ID, new AttributeItem());
            _dic.Add(Shield_ID, new AttributeItem());
        }

        public void ResetAttribute(int key, long value, long min, long max)
        {
            if (!_dic.ContainsKey(key))
            {
                Debug.LogError($"Formula 属性不存在,key:{key}");
                return;
            }

            _dic[key].UnsubscribeAll();
            _dic[key].Min = min;
            _dic[key].Max = max;
            _dic[key].Value = value;
        }

        public void ResetAttribute(EAttributeKey key, long value, long min, long max)
        {
            ResetAttribute((int)key, value, min, max);
        }

        public void SetValue(int key, long value)
        {
            if (!_dic.ContainsKey(key))
            {
                Debug.LogError($"Formula 属性不存在,key:{key}");
                return;
            }

            _dic[key].Value = value;
        }

        public void SetValue(EAttributeKey key, long value)
        {
            SetValue((int)key, value);
        }

        public void AddValue(EAttributeKey key, long addValue)
        {
            AddValue((int)key, addValue);
        }

        public void AddValue(int key, long addValue)
        {
            SetValue(key, GetValue(key) + addValue);
        }

        public long GetValue(int key)
        {
            if (!_dic.ContainsKey(key))
            {
                Debug.LogError($"Formula 属性不存在,key:{key}");
                return 0;
            }

            return _dic[key].Value;
        }

        public long GetValue(EAttributeKey key)
        {
            return GetValue((int)key);
        }

        public IAttribute GetAttribute(int key)
        {
            if (!_dic.ContainsKey(key))
            {
                Debug.LogError($"Formula 属性不存在,key:{key}");
                return null;
            }

            return _dic[key];
        }

        public IAttribute GetAttribute(EAttributeKey key)
        {
            return GetAttribute((int)key);
        }

        public void OnReset()
        {
            foreach (var key in Enum.GetValues(typeof(EAttributeKey)))
            {
                ResetAttribute((EAttributeKey)key, 0, 0, int.MaxValue);
            }
            ResetAttribute(HP_ID, 0, 0, int.MaxValue);
            ResetAttribute(Shield_ID, 0, 0, int.MaxValue);
        }

        public void ResetHP(long value, long min, long max)
        {
            HPAttribute.Min = min;
            HPAttribute.Max = max;
            HPAttribute.Value = CombatUtility.Clamp(value, min, max);
        }

        public void AddHP(IEntity source, long addValue)
        {
            AddValue(HP_ID, addValue);
            if (addValue < 0 && source != null)
            {
                _entity.EventOnTakeDamage?.Invoke(source, _entity, addValue);
            }
        }
        public void AddShield(long addValue) => AddValue(Shield_ID, addValue);
    }
}

