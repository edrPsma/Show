using System.Collections;
using System.Collections.Generic;
using Combat;
using HFSM;
using UnityEngine;

namespace Prologue
{
    public class PrologueEndNode : BasePrologueNode
    {
        public PrologueEndNode(bool hasExitTime = false) : base(hasExitTime) { }

        IPrologueSystem _prologueSystem => this.GetSystem<IPrologueSystem>();

        protected override void OnEnter()
        {
            _prologueSystem.StopPrologue();
            this.SendCommand<OpeningAnimationEndCommand>();
        }
    }
}