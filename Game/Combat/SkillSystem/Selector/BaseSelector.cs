using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public abstract class BaseSelector : Controller, ISelector
    {
        public abstract void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor);

        public abstract void Select(SkillInfo info, ISkillExcutor skillExcutor, HashSet<EntityInfo> infos);
    }
}