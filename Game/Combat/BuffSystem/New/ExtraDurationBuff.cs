using System.Collections;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.ExtraDuration)]
    public class ExtraDurationBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            int time = parms[0];//提升的时间（毫秒）
            int type = parms[1];//类型

            if (type == 1)
            {
                float addValue = time * 0.001f * buff.Multiple;

                buff.Duration.Value += addValue;
            }
            else if (type == 2)
            {
                buff.Duration.Value += time * 0.001f;
            }
            else
            {
                Debug.LogError($"Combat 没有该类型,Type:{type}");
            }
        }
    }
}