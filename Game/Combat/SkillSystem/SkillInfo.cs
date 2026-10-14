using System;
using System.Collections;
using System.Collections.Generic;
using Ev;
using Sirenix.OdinInspector;
using Skill;
using UnityEngine;

namespace Combat
{
    public class SkillInfo : Controller, IResettable
    {
        public DTSkill_base Data;
        public SkillConfig Config;
        public LayerMask LayerMask;
        public bool TargetIsPlayer;
        public bool TargetIsFriendly => Data.Target == DTSkill_base.ETarget.You_jun;
        [HideInInspector] public EntityInfo Owner;
        public bool IsSingleTarget => Config != null ? Config.DamageAreaType == ESelectType.SingleTarget : false;// 是否单体锁定技能
        public int RefCount;// 引用计数,避免技能信息回收时，技能执行器仍在使用而导致的bug
        public float Multiple => GameTools.CalSkillMultiple(Data.ID, SkillLevel);
        public int SkillLevel = 1;

        public void OnReset()
        {
            Data = null;
            Config = null;
            Owner = default;
            SkillLevel = 1;
        }
    }
}