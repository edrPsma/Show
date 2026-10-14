using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    [Selector(ESelectType.SingleTarget)]
    public class SingleTargetSelector : BaseSelector
    {
        public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor) { }

        public override void Select(SkillInfo info, ISkillExcutor skillExcutor, HashSet<EntityInfo> infos)
        {
            infos.Add(skillExcutor.LockTarget);
        }
    }
}