using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prologue
{
    public class SetOpeningAnimationStateCommand : AbstractCommand
    {
        IRoleModel _roleModel => this.GetModel<IRoleModel>();

        protected override void OnExecute()
        {
            _roleModel.OpeningAnimation = true;
        }
    }
}