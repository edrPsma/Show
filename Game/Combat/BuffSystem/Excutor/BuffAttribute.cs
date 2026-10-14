using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class BuffAttribute : ReflectionAttribute
    {
        public BuffAttribute(EBuffExcutorType key) : base(typeof(IBuffExcutor), key, 100)
        {
        }
    }
}