using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.SpawnEntity)]
    public class SpawnEntityHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new SpawnEntityHandler();

        protected override void OnStart()
        {
            Info.EntityGenerator.Spawn();

            Finish = true;
        }
    }
}