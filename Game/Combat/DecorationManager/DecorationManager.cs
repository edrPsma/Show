using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using Spine.Unity;
using UnityEngine;

namespace Combat
{
    public class DecorationManager : Controller
    {
        IEquipSytem _equipSytem => this.GetSystem<IEquipSytem>();
        static ObjectPool<DecorationItem> _pool;
        Dictionary<EDecorationType, DecorationItem> _decDic;
        public IEntity Entity;
        public bool IsActive;

        static DecorationManager()
        {
            _pool = GameEntry.Pool.GetOrCreatePool<DecorationItem>(EPoolType.Scalable, 10) as ObjectPool<DecorationItem>;
            _pool.OnReleaseEvent += item => item.OnReset();
        }

        public DecorationManager()
        {
            _decDic = new Dictionary<EDecorationType, DecorationItem>();
        }

        public void Init(IEntity entity, bool registerEvent)
        {
            Entity = entity;
            IsActive = true;
            RegisterEvent(registerEvent);
            entity.AnimationController.OnPlayAnimation += OnPlayAnimation;
        }

        void RegisterEvent(bool register)
        {
            if (!register) return;

            GameEntry.Event.Register<EventEquipChange>(OnEquipChange);
            GameEntry.Event.Register<EventChangeWing>(OnWingChange);
            GameEntry.Event.Register<EventChangeMounts>(OnMountChange);
        }

        public void Clear()
        {
            IsActive = false;
            ClearAllPrefab();
            GameEntry.Event.UnRegister<EventEquipChange>(OnEquipChange);
            GameEntry.Event.UnRegister<EventChangeWing>(OnWingChange);
            GameEntry.Event.UnRegister<EventChangeMounts>(OnMountChange);
        }

        private void OnMountChange(EventChangeMounts e)
        {
            SetMount(e.MountID);
        }

        private void OnPlayAnimation(EAnimationName name, SkeletonAnimation skeletonAnimation)
        {
            if (_decDic.ContainsKey(EDecorationType.Mounts) && _decDic[EDecorationType.Mounts].SkeletonAnimation != null)
            {
                if (name == EAnimationName.run)
                {
                    _decDic[EDecorationType.Mounts].SkeletonAnimation?.AnimationState.SetAnimation(0, "speed", true);
                    skeletonAnimation.AnimationState.SetAnimation(0, EAnimationName.idle.ToString(), true);
                }
                else
                {
                    bool loop = name == EAnimationName.idle || name == EAnimationName.run;
                    _decDic[EDecorationType.Mounts].SkeletonAnimation.AnimationState.SetAnimation(0, EAnimationName.idle.ToString(), true);
                    skeletonAnimation.AnimationState.SetAnimation(0, name.ToString(), loop);
                }
            }
            else
            {
                bool loop = name == EAnimationName.idle || name == EAnimationName.run;
                skeletonAnimation.AnimationState.SetAnimation(0, name.ToString(), loop);
            }
        }

        private void OnEquipChange(EventEquipChange e)
        {
            SetEquip(e.EquipID);
        }

        private void OnWingChange(EventChangeWing e)
        {
            SetWing(e.WingID);
        }

        public void SetEquip(int id)
        {
            var data = DataTable.GetItem<DTEquip_weapon>(id);
            if (data == null) return;

            DTEquip.EType type = DataTable.GetItem<DTEquip>(id).Type;

            string modelStr = _equipSytem.ReadEquipWeaponField<string>(id, EDataTableField.equipWeaponModel, Entity.Carrer);
            if (type == DTEquip.EType.Zhu_wu_qi)
            {
                Entity.AnimationController.ReBindSimpleAttackEvent(id);// 主武器重置普通攻击
                SetDecoration(EDecorationType.PrimaryWeapon, id, $"prefab/Weapons/{modelStr}", Entity.AnimationController.WeaponTrans_L.Transform, 1, prefab => SetEquipOrder(prefab, Entity.AnimationController.WeaponTrans_L));
            }
            else if (type == DTEquip.EType.Fu_wu_qi)
            {
                SetDecoration(EDecorationType.SecondaryWeapon, id, $"prefab/Weapons/{modelStr}", Entity.AnimationController.WeaponTrans_R.Transform, 1, prefab => SetEquipOrder(prefab, Entity.AnimationController.WeaponTrans_R));
            }
            else if (type == DTEquip.EType.Yi_fu)
            {
                // TODO 衣服特殊处理
            }
        }

        void SetEquipOrder(GameObject gameObject, PartInfo partInfo)
        {
            SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = partInfo.SortingOrder + 1;
            }
        }

        public void SetWing(int wingID)
        {
            var data = DataTable.GetItem<DTWing>(wingID);

            // 史莱姆不装备翅膀
            if (data == null)
            {
                SetDecoration(EDecorationType.Wing, wingID, null, Entity.AnimationController.WingTrans);
            }
            else
            {
                int occ = this.GetModel<IRoleModel>().PlayerData.Occupation;
                int index = Mathf.Clamp(occ <= 10 ? 0 : occ % 10, 0, 2);// 根据职业计算翅膀缩放

                List<float> scales = DataTable.GetItem<Global>("Wing_model_zoom").ToValue<List<float>>();
                SetDecoration(EDecorationType.Wing, wingID, $"prefab/spine/character/wing/{data.Model}", Entity.AnimationController.WingTrans, scales[index], prefab =>
                {
                    MeshRenderer meshRenderer = prefab.GetComponentInChildren<MeshRenderer>();
                    meshRenderer.sortingOrder = -1;
                });
            }
        }

        public void SetMount(int mountID)
        {
            var data = DataTable.GetItem<DTMount>(mountID);
            if (data == null)
            {
                Entity.AnimationController.WearMount(null);
                SetDecoration(EDecorationType.Mounts, mountID, null, Entity.AnimationController.MountTrans);
                Entity.AnimationController.Transform.localPosition = Vector3.zero;
            }
            else
            {
                Entity.AnimationController.WearMount(null);
                SetDecoration(EDecorationType.Mounts, mountID, $"prefab/spine/character/mount/{data.Model}", Entity.AnimationController.MountTrans, data.zoom_model, prefab =>
                {
                    var data = DataTable.GetItem<DTMount>(mountID);
                    MeshRenderer meshRenderer = prefab.GetComponentInChildren<MeshRenderer>();
                    if (data.stratum == DTMount.Estratum.Wan_jia_zai_qian)
                    {
                        meshRenderer.sortingOrder = -10;
                    }
                    else
                    {
                        meshRenderer.sortingOrder = 10;
                    }
                    Entity.AnimationController.WearMount(_decDic[EDecorationType.Mounts].SkeletonAnimation);
                });
                Entity.AnimationController.Transform.localPosition = new Vector3(0, 0.15f, 0);
            }
        }

        public int GetPartIDByDecorationType(EDecorationType type)
        {
            if (_decDic.ContainsKey(type))
            {
                return _decDic[type].ID;
            }
            else
            {
                return 0;
            }
        }

        void SetDecoration(EDecorationType type, int id, string path, Transform root, float scale = 1, Action<GameObject> callBack = null)
        {
            if (!IsActive) return;

            if (_decDic.ContainsKey(type))
            {
                _pool.Release(_decDic[type]);
                _decDic[type] = _pool.SpawnByType();
                _decDic[type].ID = id;
                _decDic[type].LoadModel(path, root, scale, callBack);
            }
            else
            {
                DecorationItem decorationItem = _pool.SpawnByType();
                decorationItem.ID = id;
                _decDic.Add(type, decorationItem);
                decorationItem.LoadModel(path, root, scale, callBack);
            }
        }

        void ClearAllPrefab()
        {
            foreach (var item in _decDic)
            {
                _pool.Release(item.Value);
            }
            _decDic.Clear();
        }
    }

    public enum EDecorationType
    {
        /// <summary>
        /// 主武器
        /// </summary>
        PrimaryWeapon = 0,

        /// <summary>
        /// 副武器
        /// </summary>
        SecondaryWeapon = 1,

        /// <summary>
        /// 翅膀
        /// </summary>
        Wing = 2,

        /// <summary>
        /// 坐骑
        /// </summary>
        Mounts = 3,
    }

    public class DecorationItem
    {
        /// <summary>
        /// 加载任务ID
        /// </summary>
        public int LoadTaskID;

        /// <summary>
        /// 装饰物品
        /// </summary>
        public GameObject GameObject;

        public SkeletonAnimation SkeletonAnimation;

        public int ID;

        float _scale;

        public void OnReset()
        {
            GameEntry.Prefab.Cancel(ref LoadTaskID);
            GameEntry.Prefab.Release(GameObject, true);
            GameObject = null;
            SkeletonAnimation = null;
            _scale = 1;
            ID = 0;
        }

        public void LoadModel(string path, Transform root, float scale, Action<GameObject> callBack)
        {
            _scale = scale;
            if (string.IsNullOrEmpty(path))
            {
                GameEntry.Prefab.Release(GameObject, true);
                GameObject = null;
                SkeletonAnimation = null;
            }
            else
            {
                LoadTaskID = GameEntry.Prefab.Instantiate(path, prefab => OnLoadOver(prefab, callBack), root, EPrefabTag.Residency);
            }
        }

        private void OnLoadOver(GameObject prefab, Action<GameObject> callBack)
        {
            GameObject = prefab;
            SkeletonAnimation = prefab.GetComponent<SkeletonAnimation>();
            SkeletonAnimation?.AnimationState.SetAnimation(0, EAnimationName.idle.ToString(), true);
            prefab.transform.localScale = _scale * Vector3.one;
            callBack?.Invoke(prefab);
        }
    }
}