using System.Collections;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Combat
{
    [Buff(EBuffExcutorType.AttributeModification)]
    public class AttributeModificationBuff : BuffExcutor
    {
        protected override void OnStart(Buff buff, int id, List<int> parms)
        {
            int attributeID = parms[0];// 属性ID
            int value = parms[1];// 提升的值
            int type = parms[2];// 提升类型


            SetAttribute(buff, type, value, attributeID, 1);
        }

        protected override void OnOver(Buff buff, int id, List<int> parms)
        {
            int attributeID = parms[0];
            int value = parms[1];
            int type = parms[2];

            SetAttribute(buff, type, value, attributeID, -1);
        }

        protected override void OnInterrupt(Buff buff, int id, List<int> parms)
        {
            int attributeID = parms[0];
            int value = parms[1];
            int type = parms[2];

            SetAttribute(buff, type, value, attributeID, -1);
        }

        void SetAttribute(Buff buff, int type, int value, int attributeID, int coefficient)
        {
            if (type == 1)
            {
                long addValue = (long)(buff.Multiple * value * coefficient);
                buff.Owner.AttributeSet.AddValue(attributeID, addValue);
            }
            else if (type == 2)
            {
                long addValue = value * coefficient;
                buff.Owner.AttributeSet.AddValue(attributeID, addValue);
            }
            else
            {
                Debug.LogError($"Combat 没有其他参数类型,Type:{type}");
            }
        }
    }
}