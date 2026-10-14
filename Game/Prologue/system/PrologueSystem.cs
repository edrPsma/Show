using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Task;
using UnityEngine;

namespace Prologue
{
    [System(typeof(IPrologueSystem))]
    public class PrologueSystem : AbstractSystem, IPrologueSystem
    {
        PrologueFSM _fsm;
        int _fsmTaskID;

        protected override void OnInit()
        {
            GameEntry.Task.AddTask(FSMUpdate).Run();
        }

        public void StartPrologue()
        {
            _fsm = new PrologueFSM();
            _fsm.BuildNode(GameEntry.Scene.GetScene<PrologueScene>().PlotConfig.Lines);
            _fsm.Enter();
            _fsmTaskID = GameEntry.Task.AddTask(FSMUpdate).SetRepeatTimes(-1).Run();
        }

        private void FSMUpdate(TaskInfo info)
        {
            _fsm?.Excute();
        }

        public void StopPrologue()
        {
            _fsm.Exit();
            _fsm = null;
            GameEntry.Task.CancelTask(ref _fsmTaskID);
            GameEntry.Procedure.TransitionImmediately(EGameState.EnterGame);
        }
    }
}