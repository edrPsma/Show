using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.Dialogue)]
    public class DialogueHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new DialogueHandler();

        protected override void OnStart()
        {
            base.OnStart();
            _prologueDailoguePanel.SetButtonAction(() => Finish = true);
            _prologueDailoguePanel.ShowDialogue(Info.DialogueType, Info.Name, Info.Content);
        }
    }
}