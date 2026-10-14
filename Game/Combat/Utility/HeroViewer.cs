using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Combat.CombatInformation;
using Network.Messages;
using Rank;
using Sirenix.OdinInspector;
using UnityEngine;

public class HeroViewer : MonoController
{
    IEntityFactory _entityFactory => this.GetSystem<IEntityFactory>();
    IRoleModel _roleModel => this.GetModel<IRoleModel>();
    IDetailSystem _detailSystem => this.GetSystem<IDetailSystem>();

    IEntity _entity;
    DecorationManager _decorationManager;
    long _uid;

    public void Show(long uid)
    {
        _uid = uid;

        _detailSystem.QueryDetailModel(uid, OnDetailCallBack);
    }

    private void OnDetailCallBack(MSG_DetailModel e)
    {
        if (this == null) return;
        if (_uid != e.m_uid) return;

        int playerID = _roleModel.CalPlayerID(e.m_sex, e.m_career, e.m_careerStage);
        ShowHero(playerID);
        SetWing(e.m_showWing);
        //SetMount(e.m_showMount);

        CombatInfoTool.GetWeapons(e, out int primaryWeapon, out int secondaryWeapon);
        SetPrimaryWeapon(primaryWeapon);
        SetSecondaryWeapon(secondaryWeapon);
    }

    [Button]
    public void ShowHero(int heroID)
    {
        if (_entity != null)
        {
            _entityFactory.Release(_entity);
        }

        _entity = _entityFactory.Create(ECombatType.TenSpeed, EEntityType.Viewer, heroID, true);
        Get<ModelViewer>("ModelViewer").ShowModel(_entity.GameObject);
        _entity.AnimationController.PlayAnimation(EAnimationName.idle);
        _decorationManager = _entity.Blackboard.GetGlobalReferenceValue<DecorationManager>(EntityBlackboardDefine.DECORATION_MANAGER);
    }

    [Button]
    public void SetWing(int wingID)
    {
        if (_entity == null) return;

        _decorationManager.SetWing(wingID);
    }

    [Button]
    public void SetMount(int mountID)
    {
        if (_entity == null) return;

        _decorationManager.SetMount(mountID);
    }

    [Button]
    public void SetPrimaryWeapon(int weaponID)
    {
        if (_entity == null) return;

        _decorationManager.SetEquip(weaponID);
    }

    [Button]
    public void SetSecondaryWeapon(int weaponID)
    {
        if (_entity == null) return;

        _decorationManager.SetEquip(weaponID);
    }

    protected override void Destroy()
    {
        base.Destroy();
        if (_entity != null)
        {
            _entityFactory.Release(_entity);
        }
    }
}
