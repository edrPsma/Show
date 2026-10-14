using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IImpactor
    {
        void Execute(SkillExcutor excutor, float[] param);
    }
}