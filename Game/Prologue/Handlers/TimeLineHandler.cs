using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Prologue
{
    [Plot(EPlotType.TimeLine)]
    public class TimeLineHandler : BasePlotHandler
    {
        int _taskID;
        public override IPlotHandler New() => new TimeLineHandler();

        protected override void OnStart()
        {
            Info.PlayableDirector.Play();
            _taskID = GameEntry.Task.AddTask(_ =>
            {
                Finish = true;
            }).Delay(TimeSpan.FromSeconds(Info.PlayableDirector.duration)).Run();
        }

        protected override void OnEnd()
        {
            GameEntry.Task.CancelTask(ref _taskID);
        }
    }
}