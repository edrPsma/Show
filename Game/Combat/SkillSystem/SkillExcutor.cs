using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace Combat
{
    public abstract class SkillExcutor : Controller, ISkillExcutor
    {
        public bool Active { get; private set; }
        public EntityInfo LockTarget { get; private set; }
        public SkillInfo SkillInfo { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        public bool DamageStart { get; private set; }
        public ECombatType CombatType { get; set; }
        public float Direction { get; set; }


        protected Vector3 _lockTargetPos;
        protected Vector3 _lockTargetBodyPos;
        protected Vector3 _lockTargetHeadPos;

        public HashSet<EntityInfo> Targets;//目标列表
        public List<EffectorItem> Effectors;
        private ISelector _selector;
        protected GameObject _prefab;

        #region 修正
        public float DamageRatio;// 伤害倍率
        public float Duration;// 持续时间
        public float Interval;// 间隔
        public Dictionary<EAttributeKey, int> ExtraAttribute;

        public float AttackTime;// 攻击总时间
        public float AttackTimer;// 攻击时间计时器
        public float DelayTimer;// 延迟时间计时器
        public float LiveTime;// 技能存在时间
        #endregion

        int _prefabLoadTaskID;
        bool _audioPlay;

        public SkillExcutor()
        {
            Targets = new HashSet<EntityInfo>();
            Effectors = new List<EffectorItem>();
            ExtraAttribute = new Dictionary<EAttributeKey, int>();
        }

        void ISkillExcutor.Start(ECombatType combatType, EntityInfo lockTarget, SkillInfo skillInfo)
        {
            CombatType = combatType;
            Active = true;
            LockTarget = lockTarget;
            SkillInfo = skillInfo;
            skillInfo.RefCount++;
            AttackTime = 0;
            AttackTimer = 0;
            DelayTimer = 0;
            LiveTime = 0;
            _audioPlay = false;
            Rotation = Quaternion.identity;
            DamageStart = false;
            Direction = (lockTarget.Position - Position).x > 0 ? 1 : -1;
            _selector = this.GetSystem<ISkillFactory>().GetSelector(skillInfo.Config.DamageAreaType);
            this.GetSystem<ISkillFactory>().GetSkillEffector(skillInfo, Effectors);
            LoadPrefab();
            SelectTargets();
            CalPosOnStart();
            TakeEffect();
            OnStart();
        }

        public abstract ISkillExcutor New();

        public void DebugSkill()
        {
            _selector.DebugDamageArea(SkillInfo, this);
        }

        void ISkillExcutor.Update(float deltaTime)
        {
            if (!Active) return;

            // 顺序不能动
            CalPos();
            OnUpdate(deltaTime);
            ExcuteEffects(deltaTime);
            PlayAudio();
            LiveTime += deltaTime;
        }

        public void Dispose()
        {
            if (!Active)
            {
                Debug.LogError("技能重复回收");
                return;
            }

            OnDispose();
            Active = false;
            // DisplayAllTrail(false);
            LockTarget = default;
            Targets.Clear();
            ExtraAttribute.Clear();
            _selector = null;
            this.GetSystem<ISkillFactory>().RecycleEffectorItems(Effectors);
            Effectors.Clear();
            GameEntry.Prefab.Cancel(ref _prefabLoadTaskID);
            GameEntry.Prefab.Release(_prefab, false);
            _prefab = null;

            int skillID = SkillInfo.Data.ID;
            SkillInfo.RefCount--;
            SkillInfo = null;
            CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(CombatType);
            combatManager.SkillManager.RecycleSkill(this, skillID);
        }


        protected virtual void OnStart() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnDispose() { }

        public void SetPosition(Vector3 position)
        {
            Position = position;

            if (_prefab != null)
            {
                _prefab.transform.position = position;
            }
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
            if (_prefab != null)
            {
                _prefab.transform.rotation = rotation;
            }
        }

        public void SetScale(Vector3 scale)
        {
            Scale = scale;
            if (_prefab != null)
            {
                _prefab.transform.localScale = scale;
            }
        }

        public virtual void SelectTargets()
        {
            Targets.Clear();
            _selector.Select(SkillInfo, this, Targets);
        }

        public void StartDamage()
        {
            if (DamageStart) return;

            DamageStart = true;
            ShowExplosionEffects(CombatType);
            Effectors[0].Effector.As<IReleaseSkillEffector>().TakeEffect(this, SkillInfo.Owner, null);
        }

        void ExcuteEffects(float deltaTime)
        {
            for (int i = 0; i < Effectors.Count; i++)
            {
                if (!Active) return;

                Effectors[i].Effector.Excute(this, SkillInfo.Owner, deltaTime, Effectors[i].EffectValue);
            }
        }

        void TakeEffect()
        {
            for (int i = 1; i < Effectors.Count; i++)
            {
                if (Effectors[i].TriggerTiming != DTSkill_effect.ETime.Shi_fang_hou) continue;

                Effectors[i].Effector?.As<IReleaseSkillEffector>()?.TakeEffect(this, SkillInfo.Owner, Effectors[i].EffectValue);
            }
        }

        void PlayAudio()
        {
            if (SkillInfo == null) return;
            if (_audioPlay) return;
            if (string.IsNullOrEmpty(SkillInfo.Config.SkillAudioPath)) return;

            if (LiveTime >= SkillInfo.Config.AudioDelay)
            {
                _audioPlay = true;
                GameEntry.Audio.PlayEffect(SkillInfo.Config.SkillAudioPath);
            }
        }

        void CalPos()
        {
            if (SkillInfo != null && LockTarget.IsValid)
            {
                _lockTargetPos = LockTarget.Position;
                _lockTargetBodyPos = LockTarget.Entity.AnimationController.BodyTrans.position;
                _lockTargetHeadPos = LockTarget.Entity.AnimationController.HeadTrans.position;
            }
        }

        void CalPosOnStart()
        {
            if (LockTarget.IsValid)
            {
                _lockTargetPos = LockTarget.Position;
                _lockTargetBodyPos = LockTarget.Entity.AnimationController.BodyTrans.position;
                _lockTargetHeadPos = LockTarget.Entity.AnimationController.HeadTrans.position;
            }
            else
            {
                _lockTargetPos = LockTarget.StartPosition;
                _lockTargetBodyPos = LockTarget.StartBodyPosition;
                _lockTargetHeadPos = LockTarget.StartHeadPosition;
            }
        }

        void LoadPrefab()
        {
            _prefabLoadTaskID = GameEntry.Prefab.Instantiate(SkillInfo.Config.SkillPrefabPath, OnLoadOver, null, EPrefabTag.Combat);
        }

        void ShowExplosionEffects(ECombatType combatType)
        {
            float scale = SkillInfo.Config.SkillBoomPrefabScale;
            GameEntry.Prefab.Instantiate(SkillInfo.Config.SkillBoomPrefabPath, prefab =>
            {
                CombatManager combatManager = this.GetSystem<ICombatSystem>().GetCombatManager(combatType);
                prefab.transform.position = Position;
                prefab.transform.localScale = scale * Vector3.one;

                ParticleInformation info = prefab.GetComponent<ParticleInformation>();

                float speed = combatManager.GameSpeed.Value;

                info.Play(speed);
                GameEntry.Task.AddTask(_ =>
                {
                    if (GameEntry.Prefab.IsSpawnFromPool(prefab))
                    {
                        GameEntry.Prefab.Release(prefab, false);
                    }
                }).SetName("Explosion Recycle").Delay(TimeSpan.FromSeconds(info.Duration * 1f / speed)).Run();
            }, null, EPrefabTag.Combat);
        }

        private void OnLoadOver(GameObject prefab)
        {
            _prefab = prefab;
            SetPosition(Position);
            SetRotation(Rotation);
            SetScale(Scale);
            OnPrefabLoadOver();
        }

        public virtual void OnTakeDamage(EntityInfo target)
        {

        }

        protected virtual void OnPrefabLoadOver() { }
    }
}