using System;
using System.Collections;
using System.Collections.Generic;
using Ev;
using GameBase.ResMgr;
using Pool;
using UnityEngine;
using Spine.Unity;
using Spine;
using Sirenix.OdinInspector;

namespace Combat
{
    public class AnimationController : MonoController, IAnimationController, IResettable
    {
        static ObjectPool<AnimationEventInfo> _eventPool;
        public EAnimationName CurAnimationName { get; private set; } = EAnimationName.idle;
        public IEntity Entity { get; private set; }
        public bool InitDone { get; private set; }
        IUnRegister _gameSpeedUnRegister;
        SkeletonAnimation _skeletonAnimation;
        [ShowInInspector] Dictionary<EAnimationName, List<AnimationEventInfo>> _eventDic;

        #region Part
        BoneFollower _body;
        BoneFollower _head;
        BoneFollower _hat;
        BoneFollower _wing;
        BoneFollower _model;

        public Transform BodyTrans => _body.transform;
        public Transform HeadTrans => _head.transform;
        public PartInfo WeaponTrans_L { get; private set; }
        public PartInfo WeaponTrans_R { get; private set; }
        public Transform HatTrans => _hat.transform;
        public Transform WingTrans => _wing.transform;

        public Transform MountTrans { get; private set; }

        public Transform ModelTrans => _model.transform;

        public Transform Transform => transform;

        public Action<EAnimationName, SkeletonAnimation> OnPlayAnimation { get; set; }
        #endregion

        IEquipSytem _equipSytem => this.GetSystem<IEquipSytem>();
        Dictionary<string, float> _animtionDurationDic;
        public float BaseSpeed = 1;
        public float AttackSpeed = 1;
        HashSet<string> _attackSpeddEffectList;// 受攻速影响的动作列表
        List<AnimationEventInfo> _simpleAttackEventInfos;// 普通攻击事件信息
        AnimationEventInfo _simpleAttackOverEventInfo;// 普通攻击结束事件信息
        int _loadModelTask;
        float _scale;
        [SerializeField] int _equipID;


        protected override void OnAwake()
        {
            if (_eventPool == null)
            {
                _eventPool = PoolManager.Instance.GetOrCreatePool<AnimationEventInfo>(EPoolType.Scalable, 30) as ObjectPool<AnimationEventInfo>;
                _eventPool.OnReleaseEvent += info => info.Reset();
            }

            _simpleAttackEventInfos = new List<AnimationEventInfo>();
            _attackSpeddEffectList = new HashSet<string>();
            _eventDic = new Dictionary<EAnimationName, List<AnimationEventInfo>>();
            Entity = GetComponentInParent<IEntity>();
            _animtionDurationDic = new Dictionary<string, float>();

            _body = transform.Find<BoneFollower>("body");
            _head = transform.Find<BoneFollower>("head");
            _head.followBoneRotation = false;
            WeaponTrans_L = new PartInfo();
            WeaponTrans_R = new PartInfo();
            WeaponTrans_L.BoneFollower = transform.Find<BoneFollower>("weapon_l");
            WeaponTrans_R.BoneFollower = transform.Find<BoneFollower>("weapon_r");

            _hat = transform.Find<BoneFollower>("hat");
            _wing = transform.Find<BoneFollower>("wing");
            MountTrans = transform.Find("mount");
            _model = transform.Find<BoneFollower>("model");
            _model.followZPosition = false;
        }

        void Init(GameObject model)
        {
            SetModel(model.GetComponent<SkeletonAnimation>());
        }

        public void SetModel(SkeletonAnimation skeletonAnimation)
        {
            skeletonAnimation.SetParent(_model.transform);
            _loadModelTask = 0;
            InitDone = true;
            _skeletonAnimation = skeletonAnimation;
            _skeletonAnimation.Initialize(true);
            _skeletonAnimation.Skeleton.SetToSetupPose();
            _skeletonAnimation.transform.localScale = Vector3.one * _scale;

            BindAnimationEvent(Entity.SkillOwner.SkillInfos);
            FixedAnimationState();
            BindGameSpeed();
            GetParts();
            Entity.AttributeSet.GetAttribute(EAttributeKey.AttackSpeed).Subscribe(OnAttackSpeedChange);
            Entity.AttributeSet.GetAttribute(EAttributeKey.PetAttackSpeedBonus).Subscribe(OnPetAttackSpeedChange);
        }

        public void LoadModel(string path)
        {
            _loadModelTask = GameEntry.Prefab.Instantiate(path, Init, ModelTrans, EPrefabTag.Residency);
        }

        public void Excute(float deltaTime)
        {
            AnimationEventExcute(deltaTime);
        }

        private void OnAttackSpeedChange(long value)
        {
            long speedValue = value + Entity.AttributeSet.GetValue(EAttributeKey.PetAttackSpeedBonus);
            float atkSpd = DamageFormula.CalAttackSpeed(speedValue);
            AttackSpeed = 1f / atkSpd;

            float targetSpeed = BaseSpeed;
            if (_attackSpeddEffectList.Contains(CurAnimationName.ToString()))
            {
                targetSpeed *= AttackSpeed;
            }
            _skeletonAnimation.timeScale = targetSpeed;
        }

        private void OnPetAttackSpeedChange(long value)
        {
            long speedValue = value + Entity.AttributeSet.GetValue(EAttributeKey.AttackSpeed);
            float atkSpd = DamageFormula.CalAttackSpeed(speedValue);
            AttackSpeed = 1f / atkSpd;

            float targetSpeed = BaseSpeed;
            if (_attackSpeddEffectList.Contains(CurAnimationName.ToString()))
            {
                targetSpeed *= AttackSpeed;
            }
            _skeletonAnimation.timeScale = targetSpeed;
        }

        void GetParts()
        {
            _body.skeletonRenderer = _skeletonAnimation;
            _body.SetBone("hit");
            _body.followBoneRotation = false;

            _head.skeletonRenderer = _skeletonAnimation;
            _head.SetBone("head");
            _body.followBoneRotation = false;

            WeaponTrans_L.BoneFollower.skeletonRenderer = _skeletonAnimation;
            WeaponTrans_L.BoneFollower.SetBone("weapon_l");
            WeaponTrans_L.BoneFollower.followLocalScale = true;
            MeshRenderer l1 = _skeletonAnimation.Find<MeshRenderer>("weapon_l_1");
            MeshRenderer l2 = _skeletonAnimation.Find<MeshRenderer>("weapon_l_2");
            if (l1 && l2)
            {
                WeaponTrans_L.SortingOrder = (l1.sortingOrder + l2.sortingOrder) / 2;
            }
            else
            {
                WeaponTrans_L.SortingOrder = 0;
            }

            WeaponTrans_R.BoneFollower.skeletonRenderer = _skeletonAnimation;
            WeaponTrans_R.BoneFollower.SetBone("weapon_r");
            WeaponTrans_R.BoneFollower.followLocalScale = true;

            MeshRenderer r1 = _skeletonAnimation.Find<MeshRenderer>("weapon_r_1");
            MeshRenderer r2 = _skeletonAnimation.Find<MeshRenderer>("weapon_r_2");
            if (r1 && r2)
            {
                WeaponTrans_R.SortingOrder = (r1.sortingOrder + r2.sortingOrder) / 2;
            }
            else
            {
                WeaponTrans_R.SortingOrder = 0;
            }

            _hat.skeletonRenderer = _skeletonAnimation;
            _hat.SetBone("maozi");

            _wing.skeletonRenderer = _skeletonAnimation;
            _wing.SetBone("wing");
            _wing.followZPosition = false;
        }

        public void WearMount(SkeletonRenderer skeletonRenderer)
        {
            if (skeletonRenderer != null)
            {
                _model.skeletonRenderer = skeletonRenderer;
                _model.SetBone("point");
                _model.Initialize();
            }
            else
            {
                _model.skeletonRenderer = null;
                _model.bone = null;
                _model.Initialize();
                _model.transform.localPosition = Vector3.zero;
            }
            if (CurAnimationName == EAnimationName.run)
            {
                OnPlayAnimation.Invoke(EAnimationName.run, _skeletonAnimation);
            }
        }

        public void PlayAnimation(EAnimationName name)
        {
            if (name == EAnimationName.none) return;

            CurAnimationName = name;
            if (!IsActive()) return;
            if (!_animtionDurationDic.ContainsKey(name.ToString())) return;

            bool loop = name == EAnimationName.idle || name == EAnimationName.run;

            float speed = BaseSpeed;
            if (_attackSpeddEffectList.Contains(name.ToString()))
            {
                speed *= AttackSpeed;
            }
            _skeletonAnimation.timeScale = speed;

            if (_skeletonAnimation.AnimationName != name.ToString() || !loop)
            {
                ResetEventInfo(name);
                if (OnPlayAnimation != null)
                {
                    OnPlayAnimation.Invoke(name, _skeletonAnimation);
                }
                else
                {
                    _skeletonAnimation.AnimationState.SetAnimation(0, name.ToString(), loop);
                }
            }
        }

        public void OnReset()
        {
            _equipID = 0;
            InitDone = false;
            GameEntry.Prefab.Cancel(ref _loadModelTask);

            if (_skeletonAnimation != null)
            {
                if (GameEntry.Prefab.IsSpawnFromPool(_skeletonAnimation.gameObject))
                {
                    GameEntry.Prefab.Release(_skeletonAnimation.gameObject, true);
                }
                else
                {
                    GameObject.Destroy(_skeletonAnimation.gameObject);
                }
            }

            ClearEvent();
            CurAnimationName = EAnimationName.idle;
            ResetPart();
            _gameSpeedUnRegister?.UnRegister();
            _gameSpeedUnRegister = null;
            _skeletonAnimation = null;
            _attackSpeddEffectList.Clear();
            _simpleAttackEventInfos.Clear();
            _simpleAttackOverEventInfo = null;
            OnPlayAnimation = null;
            Transform.localPosition = Vector3.zero;
            _animtionDurationDic.Clear();
        }

        void ResetPart()
        {
            _body.bone = null;
            _head.bone = null;
            WeaponTrans_L.BoneFollower.bone = null;
            WeaponTrans_R.BoneFollower.bone = null;
            _hat.bone = null;
            _wing.bone = null;
            _model.bone = null;
            _body.skeletonRenderer = null;
            _head.skeletonRenderer = null;
            WeaponTrans_L.BoneFollower.skeletonRenderer = null;
            WeaponTrans_R.BoneFollower.skeletonRenderer = null;
            _hat.skeletonRenderer = null;
            _wing.skeletonRenderer = null;
            _model.skeletonRenderer = null;

            _body.Initialize();
            _head.Initialize();
            WeaponTrans_L.BoneFollower.Initialize();
            WeaponTrans_R.BoneFollower.Initialize();
            _hat.Initialize();
            _wing.Initialize();
            _model.Initialize();
        }

        public void BindAnimationEvent(List<SkillInfo> skills, bool reBindSimpleAttack = true)
        {
            ClearEvent();
            _animtionDurationDic.Clear();
            foreach (var item in _skeletonAnimation.SkeletonDataAsset.GetAnimationStateData().SkeletonData.Animations)
            {
                _animtionDurationDic.Add(item.Name, item.Duration);
            }

            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Config == null) continue;
                if (skills[i].Config.AnimationName == EAnimationName.none) continue;

                float duration = _animtionDurationDic.GetValue(skills[i].Config.AnimationName.ToString());

                int skilID = skills[i].Data.ID;

                if (skills[i].Data.SkillType == DTSkill_base.ESkillType.Pu_tong_gong_ji && reBindSimpleAttack && Entity.IsHero())
                {
                    BindSimpleAttackEvent(skills[i], _equipID);
                }
                else
                {
                    if (skills[i].Config.Times != null && skills[i].Config.Times.Length > 0)
                    {
                        for (int j = 0; j < skills[i].Config.Times.Length; j++)
                        {
                            AddEvent(skills[i].Config.AnimationName, skills[i].Config.Times[j] * duration, () => Entity.SkillOwner.CreateSkill(skilID));
                        }
                    }
                    AddEvent(skills[i].Config.AnimationName, duration, () => Entity.EventSource.TriggerNow(EntityEvent.SkillActionDone, skilID));
                }
                _attackSpeddEffectList.Add(skills[i].Config.AnimationName.ToString());
            }
        }

        [Button]
        public void ReBindSimpleAttackEvent(int equipID)
        {
            SkillInfo skillInfo = Entity.SkillOwner.SkillInfos.Find(item => item.Data.SkillType == DTSkill_base.ESkillType.Pu_tong_gong_ji);
            BindSimpleAttackEvent(skillInfo, equipID);
        }

        public void BindSimpleAttackEvent(SkillInfo info, int equipID)
        {
            _equipID = equipID;
            if (info == null) return;
            if (!InitDone) return;

            var equipData = DataTable.GetItem<DTEquip_weapon>(equipID);
            int skillId;
            // 没有装备或装备没有技能则使用玩家表普通攻击
            if (equipData == null)
            {
                DTPlayer table = DataTable.GetItem<DTPlayer>(Entity.EntityID);
                skillId = table.General_skill;
            }
            else
            {
                skillId = _equipSytem.ReadEquipWeaponField<int>(equipID, EDataTableField.equipWeaponSkill, Entity.Carrer);
            }

            ISkillFactory skillFactory = this.GetSystem<ISkillFactory>();

            // 清空之前绑定的事件
            ClearAttackEvent(info.Config.AnimationName);

            Entity.SkillOwner.ReleaseSkill = null;
            Entity.SkillOwner.ReleaseQueue.Remove(info);
            Entity.SkillOwner.SkillInfos.Remove(info);
            skillFactory.RecycleSkillInfo(info);

            SkillInfo skillInfo = skillFactory.SpawnSkillInfo(EntityInfo.Build(Entity), skillId);
            Entity.SkillOwner.SkillInfos.Add(skillInfo);

            SetAttackEvent(skillInfo);
        }

        void ClearAttackEvent(EAnimationName animationName)
        {
            if (_eventDic.ContainsKey(animationName))
            {
                for (int i = 0; i < _simpleAttackEventInfos.Count; i++)
                {
                    _eventDic[animationName].Remove(_simpleAttackEventInfos[i]);
                    _eventPool.Release(_simpleAttackEventInfos[i]);
                }

                _eventDic[animationName].Remove(_simpleAttackOverEventInfo);
                _eventPool.Release(_simpleAttackOverEventInfo);

                _simpleAttackEventInfos.Clear();
                _simpleAttackOverEventInfo = null;
            }
        }

        void SetAttackEvent(SkillInfo skillInfo)
        {
            EAnimationName animationName = skillInfo.Config.AnimationName;

            if (skillInfo.Config.Times == null || skillInfo.Config.Times.Length == 0)
            {
                AnimationEventInfo animationEventInfo = AddEvent(animationName, 0, () => Entity.SkillOwner.CreateSkill(skillInfo.Data.ID));
                _simpleAttackEventInfos.Add(animationEventInfo);
            }
            else
            {
                for (int i = 0; i < skillInfo.Config.Times.Length; i++)
                {
                    AnimationEventInfo animationEventInfo = AddEvent(animationName, skillInfo.Config.Times[i], () => Entity.SkillOwner.CreateSkill(skillInfo.Data.ID));
                    _simpleAttackEventInfos.Add(animationEventInfo);
                }
            }

            if (!_animtionDurationDic.ContainsKey(animationName.ToString()))
            {
                Debug.LogError($"技能配置错误,该角色没有该攻击动画,SkillID: {skillInfo.Data.ID}");
            }

            float duration = _animtionDurationDic[animationName.ToString()];
            _simpleAttackOverEventInfo = AddEvent(animationName, duration, () => Entity.EventSource.TriggerNow(EntityEvent.SkillActionDone, skillInfo.Data.ID));
        }

        bool IsActive()
        {
            if (_skeletonAnimation == null) return false;

            return isActiveAndEnabled;
        }

        void FixedAnimationState()
        {
            PlayAnimation(CurAnimationName);
        }

        public AnimationEventInfo AddEvent(EAnimationName animationName, float time, Action action)
        {
            if (!_eventDic.ContainsKey(animationName))
            {
                _eventDic.Add(animationName, new List<AnimationEventInfo>());
            }

            AnimationEventInfo animationEventInfo = _eventPool.SpawnByType();
            animationEventInfo.Time = time;
            animationEventInfo.Action = action;
            _eventDic[animationName].Add(animationEventInfo);
            return animationEventInfo;
        }

        public void ResetEventInfo(EAnimationName animationName)
        {
            if (_eventDic.ContainsKey(animationName))
            {
                foreach (var item in _eventDic[animationName])
                {
                    item.Timer = 0;
                    item.Invoke = false;
                }
            }
        }

        void AnimationEventExcute(float deltaTime)
        {
            if (_eventDic.Count <= 0) return;

            if (_eventDic.ContainsKey(CurAnimationName))
            {
                int count = _eventDic[CurAnimationName].Count;
                for (int i = 0; i < count; i++)
                {
                    if (!_eventDic.ContainsKey(CurAnimationName)) break;

                    AnimationEventInfo eventInfo = _eventDic[CurAnimationName][i];
                    if (_attackSpeddEffectList.Contains(CurAnimationName.ToString()))
                    {
                        eventInfo.Timer += deltaTime * AttackSpeed;
                    }
                    else
                    {
                        eventInfo.Timer += deltaTime;
                    }
                    if (eventInfo.Timer >= eventInfo.Time && !eventInfo.Invoke)
                    {
                        eventInfo.Invoke = true;
                        eventInfo.Action?.Invoke();
                    }
                }
            }
        }

        public void ClearEvent()
        {
            foreach (var item in _eventDic)
            {
                foreach (var info in item.Value)
                {
                    _eventPool.Release(info);
                }
            }

            _eventDic.Clear();
        }

        void BindGameSpeed()
        {
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(Entity.CombatType);
            _gameSpeedUnRegister = combatManager.GameSpeed.Subscribe(OnGameSpeedChange);
        }

        private void OnGameSpeedChange(float speed)
        {
            BaseSpeed = speed;
            float targetSpeed = BaseSpeed;
            if (_attackSpeddEffectList.Contains(CurAnimationName.ToString()))
            {
                targetSpeed *= AttackSpeed;
            }
            _skeletonAnimation.timeScale = targetSpeed;
        }

        public void SetScale(float scale)
        {
            _scale = scale;
            if (_skeletonAnimation != null)
            {
                _skeletonAnimation.transform.localScale = Vector3.one * scale;
            }
        }
    }

    public class AnimationEventInfo
    {
        public float Time;
        public float Timer;
        public Action Action;
        public bool Invoke;

        public void Reset()
        {
            Invoke = false;
            Time = 0;
            Timer = 0;
            Action = null;
        }
    }
}
