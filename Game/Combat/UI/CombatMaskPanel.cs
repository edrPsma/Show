using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Task;
using UnityEngine;

namespace Combat
{
    public class CombatMaskPanel : UIFormLogic
    {
        public override UIGroup DefultGroup => UIGroup.RearMost;
        public override string Location => "ui/Combat/CombatMaskPanel";

        protected override void OnOpen()
        {
            base.OnOpen();
            GameEntry.Task.AddTask(PlayTween).SetName("CombatMaskPanel").Delay(TimeSpan.FromSeconds(1)).Run();
        }

        void PlayTween(TaskInfo taskInfo)
        {
            Get<CanvasGroup>("mask").DOFade(0, 1).OnComplete(() =>
            {
                GameEntry.UI.Pop<CombatMaskPanel>();
            });
        }
    }

    public class CombatLoadingPanel : CombatMaskPanel
    {

    }
}