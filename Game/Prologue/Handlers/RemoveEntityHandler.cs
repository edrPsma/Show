using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.RemoveEntity)]

    public class RemoveEntityHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new RemoveEntityHandler();

        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();
        CombatManager _combatManager => _combatSystem.GetCombatManager(ECombatType.Prologue);
        protected override void OnStart()
        {
            if (Info.RemoveEntityGenerator.Entity != null)
            {
                _combatManager.RemovePlayer(Info.RemoveEntityGenerator.Entity);
                _combatManager.RemoveEnemy(Info.RemoveEntityGenerator.Entity);
            }
            Finish = true;
        }
    }
}