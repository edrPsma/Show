using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 死亡行为
    /// </summary>
    public class DeathAction : BaseAction
    {
        public override void OnStart()
        {
            _entity.AIAgent.StopBehavior();
            _entity.HitBox.SetActive(false);
            _entity.EventSource.TriggerNow(EntityEvent.Death, _entity);
        }
    }
}