using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.AIState)]
    public class AIStateHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new AIStateHandler();

        protected override void OnStart()
        {
            if (Info.OpenAI)
            {
                Info.AIEntityGenerator.Entity.AIAgent.StartBehavior();
            }
            else
            {
                Info.AIEntityGenerator.Entity.AIAgent.StopBehavior();
            }
            Finish = true;
        }
    }
}