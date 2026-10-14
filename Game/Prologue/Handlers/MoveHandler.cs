using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using DG.Tweening;
using UnityEngine;

namespace Prologue
{
    [Plot(EPlotType.Move)]
    public class MoveHandler : BasePlotHandler
    {
        public override IPlotHandler New() => new MoveHandler();

        ICombatSystem _combatSystem => this.GetSystem<ICombatSystem>();
        CombatManager _combatManager => _combatSystem.GetCombatManager(ECombatType.Prologue);

        protected override void OnStart()
        {
            PrologueScene scene = GameEntry.Scene.GetScene<PrologueScene>();
            Transform movePoint = scene.MapController.MovePoint;

            PlayAnimation(EAnimationName.run);
            movePoint.DOMoveX(movePoint.position.x + Info.MoveDistance, Info.MoveTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        PlayAnimation(EAnimationName.idle);
                        Finish = true;
                    });
        }

        void PlayAnimation(EAnimationName animationName)
        {
            IEntity[] entities = _combatManager.Players;
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i].AnimationController.PlayAnimation(animationName);
            }
        }
    }
}