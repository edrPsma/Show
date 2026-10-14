using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using HFSM;
using UnityEngine;

namespace Prologue
{
    public class PrologueStartNode : BasePrologueNode
    {
        public PrologueStartNode(bool hasExitTime = false) : base(hasExitTime) { }

        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();
        protected override void OnEnter()
        {
            base.OnEnter();
            // _combatSystem.GetCombatManager(ECombatType.Prologue).TeamManager.SpawnPlayerEntity();
            IAIAgent.StopAI = true;
        }
    }
}