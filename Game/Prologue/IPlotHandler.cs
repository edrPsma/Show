using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    public interface IPlotHandler
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        PlotInfo Info { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        bool Finish { get; set; }

        /// <summary>
        /// new方法
        /// </summary>
        /// <returns></returns>
        IPlotHandler New();

        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 帧循环
        /// </summary>
        /// <returns></returns>
        void Update();

        /// <summary>
        /// 结束
        /// </summary>
        void End();
    }
}