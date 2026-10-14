using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.Combat)]
    public class CombatHandler : BasePlotHandler
    {
        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();
        CombatManager _combatManager => _combatSystem.GetCombatManager(ECombatType.Prologue);

        public override IPlotHandler New() => new CombatHandler();

        protected override void OnStart()
        {
            IAIAgent.StopAI = false;
            _combatManager.StartTimer(Info.CombatTime);
            _combatManager.StartCombat();
            GameEntry.Event.Register<EventStopCombat>(OnCombatStop);
        }

        protected override void OnEnd()
        {
            IAIAgent.StopAI = true;
            GameEntry.Event.UnRegister<EventStopCombat>(OnCombatStop);
        }

        private void OnCombatStop(EventStopCombat e)
        {
            if (e.CombatType != ECombatType.Prologue) return;

            Finish = true;
        }
    }
}