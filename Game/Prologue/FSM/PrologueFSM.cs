using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

namespace Prologue
{
    public class PrologueFSM : BaseFSM<int>
    {
        public PrologueFSM(bool hasExitTime = false, int initialState = 0) : base(hasExitTime, initialState) { }

        public void BuildNode(List<PlotInfo> lines)
        {
            AddState(0, new PrologueStartNode());
            AddState(lines.Count + 1, new PrologueEndNode());
            for (int i = 0; i < lines.Count; i++)
            {
                int index = i + 1;

                PrologueNode state = new PrologueNode(lines[i]);
                AddState(index, state);

                AddTransition(i, index, FunTrue, false);
            }

            AddTransition(lines.Count, lines.Count + 1, FunTrue, false);
        }

        bool FunTrue() => true;
    }
}