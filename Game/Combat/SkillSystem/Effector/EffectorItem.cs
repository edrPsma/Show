using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Combat
{
    public class EffectorItem : Controller, IResettable
    {
        public int EffectId;
        public DTSkill_effect.ETime TriggerTiming;
        public ISkillEffector Effector;
        [ShowInInspector] public List<int> EffectValue;

        public EffectorItem()
        {
            EffectValue = new List<int>();
        }

        public void OnReset()
        {
            EffectId = 0;
            EffectValue.Clear();
            Effector = null;
        }
    }
}