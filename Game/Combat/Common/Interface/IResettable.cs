using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public interface IResettable
    {
        /// <summary>
        /// 重置时调用
        /// </summary>
        void OnReset();
    }
}