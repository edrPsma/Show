using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class BuilderAttribute : ReflectionAttribute
    {
        public BuilderAttribute(EEntityType entityType) : base(typeof(IEntityBuilder), entityType, 100)
        {

        }
    }
}