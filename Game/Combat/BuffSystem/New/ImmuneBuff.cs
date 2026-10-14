using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.Immune)]
    public class ImmuneBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            DTSkill_buff.ETage type = (DTSkill_buff.ETage)parms[0];// 免疫类型
            buff.Owner.BuffOwner.AddToIgnoreList(type);
        }

        protected override void OnOver(Buff buff, int id, List<int> parms)
        {
            DTSkill_buff.ETage type = (DTSkill_buff.ETage)parms[0];// 免疫类型
            buff.Owner.BuffOwner.RemoveFormIgnoreList(type);
        }
    }
}