using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Blackboard
{
    [ShowInInspector] Dictionary<Type, Dictionary<object, int>> _intValueDic;
    [ShowInInspector] Dictionary<Type, Dictionary<object, long>> _longValueDic;
    [ShowInInspector] Dictionary<Type, Dictionary<object, bool>> _boolValueDic;
    [ShowInInspector] Dictionary<Type, Dictionary<object, float>> _floatValueDic;
    [ShowInInspector] Dictionary<Type, Dictionary<object, object>> _valueDic;

    public Blackboard()
    {
        _intValueDic = new Dictionary<Type, Dictionary<object, int>>();
        _longValueDic = new Dictionary<Type, Dictionary<object, long>>();
        _boolValueDic = new Dictionary<Type, Dictionary<object, bool>>();
        _floatValueDic = new Dictionary<Type, Dictionary<object, float>>();
        _valueDic = new Dictionary<Type, Dictionary<object, object>>();
    }

    public void SetGlobalIntValue(object key, int value)
    {
        SetIntValue(GetType(), key, value);
    }

    public int GetGlobalIntValue(object key, int defultValue = 0)
    {
        return GetIntValue(GetType(), key, defultValue);
    }

    public void SetGlobalLongValue(object key, int value)
    {
        SetIntValue(GetType(), key, value);
    }

    public long GetGlobalLongValue(object key, long defultValue = 0)
    {
        return GetLongValue(GetType(), key, defultValue);
    }

    public void SetGlobalFloatValue(object key, float value)
    {
        SetFloatValue(GetType(), key, value);
    }

    public float GetGlobalFloatValue(object key, float defultValue = 0)
    {
        return GetFloatValue(GetType(), key, defultValue);
    }

    public void SetGlobalBoolValue(object key, bool value)
    {
        SetBoolValue(GetType(), key, value);
    }

    public bool GetGlobalBoolValue(object key, bool defultValue = false)
    {
        return GetBoolValue(GetType(), key, defultValue);
    }

    public void SetGlobalReferenceValue(object key, object value)
    {
        SetReferenceValue(GetType(), key, value);
    }

    public T GetGlobalReferenceValue<T>(object key) where T : class
    {
        return GetReferenceValue<T>(GetType(), key);
    }

    public void SetIntValue(Type type, object key, int value)
    {
        SetValue(type, key, value, _intValueDic);
    }

    public int GetIntValue(Type type, object key, int defultValue = 0)
    {
        return GetValue(type, key, _intValueDic, defultValue);
    }

    public void SetLongValue(Type type, object key, long value)
    {
        SetValue(type, key, value, _longValueDic);
    }

    public long GetLongValue(Type type, object key, long defultValue = 0)
    {
        return GetValue(type, key, _longValueDic, defultValue);
    }

    public void SetFloatValue(Type type, object key, float value)
    {
        SetValue(type, key, value, _floatValueDic);
    }

    public float GetFloatValue(Type type, object key, float defultValue = 0)
    {
        return GetValue(type, key, _floatValueDic, defultValue);
    }

    public void SetBoolValue(Type type, object key, bool value)
    {
        SetValue(type, key, value, _boolValueDic);
    }

    public bool GetBoolValue(Type type, object key, bool defultValue = false)
    {
        return GetValue(type, key, _boolValueDic, defultValue);
    }

    public void SetReferenceValue(Type type, object key, object value)
    {
        SetValue(type, key, value, _valueDic);
    }

    public T GetReferenceValue<T>(Type type, object key) where T : class
    {
        return GetValue(type, key, _valueDic, null) as T;
    }

    void SetValue<T>(Type type, object key, T value, Dictionary<Type, Dictionary<object, T>> dic)
    {
        if (!dic.ContainsKey(type))
        {
            dic.Add(type, new Dictionary<object, T>());
        }

        if (!dic[type].ContainsKey(key))
        {
            dic[type].Add(key, value);
        }
        else
        {
            dic[type][key] = value;
        }
    }

    T GetValue<T>(Type type, object key, Dictionary<Type, Dictionary<object, T>> dic, T defultValue)
    {
        if (!dic.ContainsKey(type))
        {
            return defultValue;
        }

        if (!dic[type].ContainsKey(key))
        {
            return defultValue;
        }
        else
        {
            return dic[type][key];
        }
    }

    public void Reset()
    {
        _intValueDic.Clear();
        _floatValueDic.Clear();
        _boolValueDic.Clear();
        _valueDic.Clear();
    }
}
