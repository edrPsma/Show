using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Combat
{
    public class HPBar : MonoController, IResettable
    {
        public IEntity Entity { get; private set; }
        [SerializeField] float _duration = 1;
        Tween _midTween;

        public void Init(IEntity entity)
        {
            Entity = entity;
            entity.AttributeSet.HPAttribute.Subscribe(OnHpChange);
        }

        public void OnReset()
        {
            Entity.AttributeSet.HPAttribute.Unsubscribe(OnHpChange);
            _midTween?.Kill();
            _midTween = null;
            Entity = null;
            GameEntry.Prefab.Release(gameObject, false);
        }

        private void OnHpChange(long value)
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(Entity.CombatType);
            float percentage = value * 1f / Entity.AttributeSet.HPAttribute.Max;
            Get<SpriteRenderer>("font").size = new Vector2(percentage, 1);
            _midTween?.Kill();
            _midTween = DOTween.To(MidSize, SetMidSize, percentage, _duration * 1f / combatManager.GameSpeed.Value).SetEase(Ease.Linear);
        }

        float MidSize()
        {
            return Get<SpriteRenderer>("mid").size.x;
        }

        void SetMidSize(float value)
        {
            Get<SpriteRenderer>("mid").size = new Vector2(value, 1);
        }

        public static void SetHPBar(string path, int dir, EntityInfo entityInfo)
        {
            GameEntry.Prefab.Instantiate(path, prefab =>
            {
                if (!entityInfo.IsValid)
                {
                    GameEntry.Prefab.Release(prefab, false);
                    return;
                }
                prefab.GetComponent<HPBar>().Init(entityInfo.Entity);
                prefab.transform.localPosition = Vector3.up * 0.02f;
                prefab.transform.localScale = new Vector3(dir, 1, 1);
                prefab.SetParent(entityInfo.Entity.AnimationController.HeadTrans);
            }, null, EPrefabTag.Residency);
        }
    }
}