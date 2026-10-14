using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.Dizziness)]
    public class DizzinessBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            buff.Owner.AIAgent.CantDoAction++;
        }

        protected override void OnOver(Buff buff, int id, List<int> parms)
        {
            buff.Owner.AIAgent.CantDoAction--;
        }
    }
}