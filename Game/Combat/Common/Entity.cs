using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Ev;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
    public class Entity : MonoController, IEntity, IResettable
    {
        [ShowInInspector, ReadOnly] public EEntityType EntityType { get; set; }
        [ShowInInspector] public int GUID { get; set; }
        public bool Active => GUID != 0;
        [ShowInInspector, ReadOnly] public bool IsPlayer { get; set; }
        public IAttributeSet AttributeSet { get; private set; }
        public IAIAgent AIAgent { get; private set; }
        public IAnimationController AnimationController { get; private set; }
        public CapsuleCollider HitBox { get; private set; }
        public GameObject GameObject => gameObject;
        public int Direction { get; private set; }
        public IEventSource EventSource { get; private set; }
        public ISkillOwner SkillOwner { get; private set; }
        public IBuffOwner BuffOwner { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsMonster { get; set; }
        public int EntityID { get; set; }
        public int Carrer { get; set; }
        [ShowInInspector] public Blackboard Blackboard { get; private set; }
        public EventBeforeCalDamage EventBeforeCalDamage { get; set; }
        public EventAfterCalDamage EventAfterCalDamage { get; set; }
        public SummonManager SummonManager { get; private set; }
        [ShowInInspector] public ECombatType CombatType { get; set; }
        public EventOnTakeDamage EventOnTakeDamage { get; set; }

        float _lifeTimer;

        public void SetDirection(int direction)
        {
            Direction = direction;
            transform.localScale = new Vector3(direction, 1, 1);
        }

        protected override void OnAwake()
        {
            AttributeSet = GetComponent<IAttributeSet>();
            AIAgent = GetComponent<IAIAgent>();
            AnimationController = GetComponentInChildren<AnimationController>();
            HitBox = GetComponentInChildren<CapsuleCollider>();
            EventSource = GameEntry.Event.CreateOrGetEvenntSource($"Entity{GetInstanceID()}");
            SkillOwner = GetComponent<SkillOwner>();
            BuffOwner = GetComponent<BuffOwner>();
            Blackboard = new Blackboard();
            SummonManager = GetComponentInChildren<SummonManager>();
        }

        public void Init()
        {
            AttributeSet.GetAttribute(EAttributeKey.Health).Subscribe(OnBasicHpChange);
            AttributeSet.GetAttribute(EAttributeKey.HealthBonus).Subscribe(OnHealthBonusChange);
            AttributeSet.GetAttribute(EAttributeKey.SkillHealthBonus).Subscribe(OnSkillHealthBonusChange);
        }

        public void Excute(float deltaTime)
        {
            SkillOwner.Excute(deltaTime);
            BuffOwner.Excute(deltaTime);
            LifeRecovery(deltaTime);
            AnimationController.Excute(deltaTime);
            EventSource.TriggerNow<IEntity>(EntityEvent.Update, this);
        }

        private void LifeRecovery(float deltaTime)
        {
            if (!Active) return;

            if (_lifeTimer <= 0)
            {
                long hp = DamageFormula.CalLifeRegeneration(AttributeSet);
                AttributeSet.AddHP(this, hp);
                _lifeTimer += 5;
            }
            _lifeTimer -= deltaTime;
        }

        #region HP Synchronous
        private void OnSkillHealthBonusChange(long value)
        {
            var basic = AttributeSet.GetValue(EAttributeKey.Health);
            var hpBonus = AttributeSet.GetValue(EAttributeKey.HealthBonus);
            var oldHpSkillBonus = AttributeSet.GetValue(EAttributeKey.SkillHealthBonus);

            var oldMax = DamageFormula.CalMaxHP(basic, hpBonus, oldHpSkillBonus);
            var newMax = DamageFormula.CalMaxHP(basic, hpBonus, value);
            ResetHP(oldMax, newMax);
        }

        private void OnHealthBonusChange(long value)
        {
            var basic = AttributeSet.GetValue(EAttributeKey.Health);
            var oldHpBonus = AttributeSet.GetValue(EAttributeKey.HealthBonus);
            var hpSkillBonus = AttributeSet.GetValue(EAttributeKey.SkillHealthBonus);

            var oldMax = DamageFormula.CalMaxHP(basic, oldHpBonus, hpSkillBonus);
            var newMax = DamageFormula.CalMaxHP(basic, value, hpSkillBonus);
            ResetHP(oldMax, newMax);
        }

        void ResetHP(double oldMax, double newMax)
        {
            var curHp = AttributeSet.HP;
            double percentage = curHp * 1d / oldMax;
            double newHp = percentage * newMax;

            AttributeSet.ResetHP(CombatUtility.CeilToLong(newHp), 0, CombatUtility.CeilToLong(newMax));
        }

        private void OnBasicHpChange(long value)
        {
            var oldBasic = AttributeSet.GetValue(EAttributeKey.Health);
            var hpBonus = AttributeSet.GetValue(EAttributeKey.HealthBonus);
            var hpSkillBonus = AttributeSet.GetValue(EAttributeKey.SkillHealthBonus);

            var oldMax = DamageFormula.CalMaxHP(oldBasic, hpBonus, hpSkillBonus);
            var newMax = DamageFormula.CalMaxHP(value, hpBonus, hpSkillBonus);
            ResetHP(oldMax, newMax);
        }
        #endregion

        public void OnReset()
        {
            StopAllCoroutines();
            EventSource.TriggerNow<IEntity>(EntityEvent.Recycle, this);
            transform.DOKill();
            HitBox.SetActive(true);
            EventSource.Clear();
            Blackboard.Reset();
            EventBeforeCalDamage = null;
            EventAfterCalDamage = null;
            EventOnTakeDamage = null;
            _lifeTimer = 0;
        }
    }
}

