using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BossWarningPanel : UIFormLogic
{
    public override UIGroup DefultGroup => UIGroup.RearMost;
    public override string Location => "ui/Combat/BossWarningPanel";

    protected override void OnOpen()
    {
        base.OnOpen();
        GameEntry.Task.AddTask(_ =>
        {
            GameEntry.UI.Pop<BossWarningPanel>();
        }).Delay(TimeSpan.FromSeconds(1.3f)).Run();
    }
}
