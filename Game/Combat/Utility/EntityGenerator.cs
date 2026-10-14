using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Combat;
using Sirenix.OdinInspector;
using Skill;
using Spine.Unity;
using UnityEngine;

public class EntityGenerator : MonoController
{
    [SerializeField] bool _runInStart;
    [SerializeField] ECombatType _combatType;
    public Entity Entity;
    [SerializeField] int _entityID;
    [SerializeField] EEntityType _entityType;
    [SerializeField] bool _isPlayer;
    [SerializeField] bool _isMonser;
    [SerializeField] SkeletonAnimation _model;
    [SerializeField] int[] _skills;
    [SerializeField] ExternalBehavior _externalBehavior;
    [SerializeField] bool _isRight;

    [SerializeField, ShowIf(nameof(IsHero))] bool _hasSkillSystem;
    [SerializeField] Vector2Int[] _attributes;
    [SerializeField] int _minHp;

    protected override void OnStart()
    {
        base.OnStart();

        if (!_runInStart) return;

        Spawn();
    }

    public void Spawn()
    {
        Entity = this.GetSystem<IEntityFactory>().CreateEmpty() as Entity;
        Entity.SetParent(transform);

        Entity.EntityID = _entityID;
        Entity.GUID = GetInstanceID();
        Entity.CombatType = _combatType;
        Entity.EntityType = _entityType;
        Entity.IsPlayer = _isPlayer;
        Entity.IsMonster = _isMonser;
        Entity.SetDirection(_isRight ? 1 : -1);
        Entity.AIAgent.Init(_externalBehavior);
        SetAttribute();
        BindEvent();
        BindSkill();
        SetLayer(Entity, _isPlayer);
        SetModel();
        SetSkillSystem();
        Entity.Init();
        Entity.AIAgent.StartBehavior();

        if (_isPlayer)
        {
            this.GetSystem<ICombatSystem>().GetCombatManager(_combatType).TeamManager.Hero = Entity;
            this.GetSystem<ICombatSystem>().GetCombatManager(_combatType).AddHero(Entity);
        }
        else
        {
            this.GetSystem<ICombatSystem>().GetCombatManager(_combatType).AddEnemy(Entity);
        }
    }

    void BindEvent()
    {
        Entity.EventSource.Register<IEntity>(EntityEvent.Death, OnDeath);
        Entity.EventSource.Register<IEntity>(EntityEvent.Recycle, OnRecycle);

        if (Entity.IsMonster)
        {
            Entity.AttributeSet.GetAttribute(EAttributeKey.MovementSpeed).Subscribe(value => OnMovementSpeedChange(Entity, value));
            Entity.AttributeSet.GetAttribute(EAttributeKey.MovementSpeedBonus).Subscribe(value => OnMovementSpeedBonusChange(Entity, value));
        }
    }

    void SetAttribute()
    {
        if (_attributes == null) return;

        for (int i = 0; i < _attributes.Length; i++)
        {
            Entity.AttributeSet.SetValue(_attributes[i].x, _attributes[i].y);
        }
        long hp = DamageFormula.CalMaxHP(Entity.AttributeSet);
        Entity.AttributeSet.ResetHP(hp, _minHp, hp);
    }

    private void OnRecycle(IEntity entity)
    {
        if (_isPlayer && IsHero())
        {
            this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType).TeamManager.SetHeroEmpty();
            entity.Blackboard.GetGlobalReferenceValue<ISkillSystem>(EntityBlackboardDefine.SKILL_SYSTEM)?.Clear();
        }
    }

    private void OnDeath(IEntity entity)
    {
        CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(entity.CombatType);
        if (_isPlayer)
        {
            if (IsHero())
            {
                combatManager.TeamManager.SetHeroEmpty();
            }
            combatManager.RemovePlayer(entity);
        }
        else
        {
            combatManager.RemoveEnemy(entity);
        }
    }

    private void OnMovementSpeedBonusChange(IEntity entity, long value)
    {
        long basic = entity.AttributeSet.GetValue(EAttributeKey.MovementSpeed);
        long speed = DamageFormula.CalMoveSpeed(basic, value);
        entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.MOVE_SPEED, (int)speed);
    }

    private void OnMovementSpeedChange(IEntity entity, long value)
    {
        long bonus = entity.AttributeSet.GetValue(EAttributeKey.MovementSpeedBonus);
        long speed = DamageFormula.CalMoveSpeed(value, bonus);
        entity.Blackboard.SetGlobalIntValue(EntityBlackboardDefine.MOVE_SPEED, (int)speed);
    }

    void BindSkill()
    {
        ISkillFactory skillFactory = this.GetSystem<ISkillFactory>();
        skillFactory.BindSkill(Entity, _skills);
    }

    void SetModel()
    {
        Entity.AnimationController.SetScale(Mathf.Abs(_model.transform.localScale.x));
        Entity.AnimationController.SetModel(_model);
    }

    void SetSkillSystem()
    {
        if (!_hasSkillSystem || !IsHero()) return;

        SkillSystem skillSystem = new SkillSystem();
        skillSystem.Init(Entity);
        Entity.Blackboard.SetGlobalReferenceValue(EntityBlackboardDefine.SKILL_SYSTEM, skillSystem);
    }

    void SetLayer(Entity entity, bool isPlayer)
    {
        if (isPlayer)
        {
            entity.HitBox.gameObject.layer = LayerDefine.PLAYER;
        }
        else
        {
            entity.HitBox.gameObject.layer = LayerDefine.MONSTER;
        }
    }

    bool IsHero()
    {
        return _entityType == EEntityType.Hero;
    }
}
