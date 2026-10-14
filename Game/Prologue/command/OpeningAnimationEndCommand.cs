using System.Collections;
using System.Collections.Generic;
using Network.Messages;
using UnityEngine;

namespace Prologue
{
    /// <summary>
    /// 开场动画播放完毕
    /// </summary>
    public class OpeningAnimationEndCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            MSG_U2GS_OpeningAnimationEnd msg = new MSG_U2GS_OpeningAnimationEnd();

            GameEntry.Net.SendMsg(msg);
        }
    }
}