using System;
using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

namespace Prologue
{
    public class PrologueNode : BaseFSMState
    {
        List<IPlotHandler> _plotHandlers;
        PlotInfo _plotInfo;
        public PrologueNode(PlotInfo info, bool hasExitTime = true) : base(hasExitTime)
        {
            _plotInfo = info;
            _plotHandlers = new List<IPlotHandler>();
            foreach (EPlotType type in Enum.GetValues(typeof(EPlotType)))
            {
                if ((info.Type & type) == info.Type)
                {
                    IPlotHandler plotHandler = GameEntry.Reflection.Get<IPlotHandler>(type)?.New();

                    if (plotHandler != null)
                    {
                        plotHandler.Info = info;
                        _plotHandlers.Add(plotHandler);
                    }
                }
            }
        }

        protected override void OnEnter()
        {
            GameEntry.UI.SearchForm<PrologueDailoguePanel>().SetButtonState(_plotInfo.ButtonState);
            for (int i = 0; i < _plotHandlers.Count; i++)
            {
                _plotHandlers[i].Start();
            }
        }

        protected override void OnExcute()
        {
            bool canExit = true;
            for (int i = 0; i < _plotHandlers.Count; i++)
            {
                _plotHandlers[i].Update();
                canExit = canExit && _plotHandlers[i].Finish;
            }

            if (canExit)
            {
                ReadyToExit();
            }
        }

        protected override void OnExit()
        {
            for (int i = 0; i < _plotHandlers.Count; i++)
            {
                _plotHandlers[i].End();
            }
        }
    }
}