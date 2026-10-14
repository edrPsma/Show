using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Combat
{
    public interface IAnimationController
    {
        /// <summary>
        /// 初始化完成
        /// </summary>
        bool InitDone { get; }

        /// <summary>
        /// 当前动画名
        /// </summary>
        EAnimationName CurAnimationName { get; }
        /// <summary>
        /// 身体
        /// </summary>
        Transform BodyTrans { get; }

        /// <summary>
        /// 头部
        /// </summary>
        Transform HeadTrans { get; }

        /// <summary>
        /// 左手武器
        /// </summary>
        PartInfo WeaponTrans_L { get; }

        /// <summary>
        /// 右手武器
        /// </summary>
        PartInfo WeaponTrans_R { get; }

        /// <summary>
        /// 帽子
        /// </summary>
        Transform HatTrans { get; }

        /// <summary>
        /// 翅膀
        /// </summary>
        Transform WingTrans { get; }

        /// <summary>
        /// 坐骑
        /// </summary>
        Transform MountTrans { get; }

        /// <summary>
        /// 模型
        /// </summary>
        Transform ModelTrans { get; }

        Transform Transform { get; }

        Action<EAnimationName, SkeletonAnimation> OnPlayAnimation { get; set; }

        /// <summary>
        /// 设置模型
        /// </summary>
        /// <param name="skeletonAnimation"></param>
        void SetModel(SkeletonAnimation skeletonAnimation);

        /// <summary>
        /// 加载模型
        /// </summary>
        /// <param name="path"></param>
        void LoadModel(string path);

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="name">动画名称</param>
        void PlayAnimation(EAnimationName name);

        /// <summary>
        /// 设置缩放
        /// </summary>
        void SetScale(float scale);

        /// <summary>
        /// 设置坐骑骨骼
        /// </summary>
        /// <param name="skeletonRenderer"></param>
        void WearMount(SkeletonRenderer skeletonRenderer);

        /// <summary>
        /// 绑定动画事件
        /// </summary>
        /// <param name="skills"></param>
        /// <param name="reBindSimpleAttack"></param>
        void BindAnimationEvent(List<SkillInfo> skills, bool reBindSimpleAttack = true);

        /// <summary>
        /// 添加动画事件
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="time"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        AnimationEventInfo AddEvent(EAnimationName animationName, float time, Action action);

        /// <summary>
        /// 重置动画事件信息
        /// </summary>
        /// <param name="animationName"></param>
        void ResetEventInfo(EAnimationName animationName);

        /// <summary>
        /// 重新绑定普通攻击事件
        /// </summary>
        void ReBindSimpleAttackEvent(int equipID);

        /// <summary>
        /// 清空动画事件
        /// </summary>
        void ClearEvent();

        /// <summary>
        /// 逻辑帧循环
        /// </summary>
        /// <param name="deltaTime"></param>
        void Excute(float deltaTime);
    }
}