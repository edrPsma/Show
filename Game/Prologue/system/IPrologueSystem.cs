using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    public interface IPrologueSystem : ISystem
    {
        void StartPrologue();

        void StopPrologue();
    }
}