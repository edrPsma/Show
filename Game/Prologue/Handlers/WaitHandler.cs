using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.Wait)]
    public class WaitHandler : IPlotHandler
    {
        public bool Finish { get; set; }
        public PlotInfo Info { get; set; }
        int _taskID;

        public IPlotHandler New() => new WaitHandler();

        public void Start()
        {
            _taskID = GameEntry.Task.AddTask(_ => Finish = true).Delay(TimeSpan.FromSeconds(Info.WaitTime)).Run();
        }

        public void Update()
        {

        }

        public void End()
        {
            GameEntry.Task.CancelTask(ref _taskID);
        }
    }
}