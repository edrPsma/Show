using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    public class SelectorAttribute : ReflectionAttribute
    {
        public SelectorAttribute(ESelectType type) : base(typeof(ISelector), type, 100) { }
    }
}