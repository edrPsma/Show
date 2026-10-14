using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    public abstract class BasePlotHandler : Controller, IPlotHandler
    {
        public PlotInfo Info { get; set; }
        public bool Finish { get; set; }
        protected PrologueDailoguePanel _prologueDailoguePanel;

        public abstract IPlotHandler New();

        void IPlotHandler.Start()
        {
            _prologueDailoguePanel = GameEntry.UI.SearchForm<PrologueDailoguePanel>();
            OnStart();
        }

        void IPlotHandler.Update()
        {
            OnUpdate();
        }

        void IPlotHandler.End()
        {
            OnEnd();
        }

        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnEnd() { }
    }
}