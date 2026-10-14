using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static DTSkill_base;

namespace Combat
{
    [CreateAssetMenu(menuName = "Combat/SkillConfig", fileName = "SkillConfig", order = 1)]
    public class SkillConfig : ScriptableObject
    {
        /// <summary>
        /// 技能规则类型
        /// </summary>
#if UNITY_EDITOR
        [InfoBox("$GetInfoBox", InfoMessageType.Error)]
#endif
        [LabelText("技能规则类型")] public ESkillRuleType RuleType;


#if UNITY_EDITOR
        string GetInfoBox()
        {
            if (SkillRuleHelper.DescDic.ContainsKey(RuleType))
            {
                return SkillRuleHelper.DescDic[RuleType];
            }
            else
            {
                return "没有参数说明";
            }
        }
#endif

        /// <summary>
        /// 规则参数
        /// </summary>
        [LabelText("技能规则参数")] public float[] RuleValues;

        /// <summary>
        /// 动画名称
        /// </summary>
        [LabelText("动画名称")] public EAnimationName AnimationName;

        /// <summary>
        /// 技能释放时间（动作百分比）
        /// </summary>
        [LabelText("技能释放时间（动作百分比）")] public float[] Times = new float[] { 0 };

        /// <summary>
        /// 伤害范围类型
        /// </summary>
        [LabelText("伤害范围类型")] public ESelectType DamageAreaType;

        /// <summary>
        /// 选择范围
        /// </summary>
        [LabelText("选择范围"), Range(0, 1.5f)] public float SelectArea;

        /// <summary>
        /// 伤害范围
        /// </summary>
        [LabelText("伤害范围"), Range(0, 1.5f)] public float DamageArea;

        /// <summary>
        /// 销毁类型
        /// </summary>
        [LabelText("销毁类型")] public EDestoryType DestoryType;

        /// <summary>
        /// 持续时间
        /// </summary>
        [LabelText("持续时间")] public float Duration;

        /// <summary>
        /// 伤害延迟
        /// </summary>
        [LabelText("伤害延迟")] public float Delay;

        /// <summary>
        /// 伤害间隔
        /// </summary>
        [LabelText("伤害间隔")] public float Interval;

        /// <summary>
        /// 动作特效路径
        /// </summary>
        [LabelText("动作特效路径")] public string ActionPrefabPath;

        /// <summary>
        /// 技能特效路径
        /// </summary>
        [LabelText("技能特效路径")] public string SkillPrefabPath;

        /// <summary>
        /// 技能释放位置调整
        /// </summary>
        [LabelText("技能释放位置调整")] public Vector3 SkillPrefabModify = Vector3.forward;// 子弹发射位置调整(x位置,y缩放,缩放)

        /// <summary>
        /// 位置调整是否作用于坐骑
        /// </summary>
        [LabelText("位置调整是否作用于坐骑")] public bool FollowMount;

        /// <summary>
        /// 技能初始位置
        /// </summary>
        [LabelText("技能初始位置")] public EBulletPlace Bullet_place = EBulletPlace.Self;

        /// <summary>
        /// 技能爆炸特效路径
        /// </summary>
        [LabelText("技能爆炸特效路径")] public string SkillBoomPrefabPath;

        [LabelText("技能爆炸特效缩放")] public float SkillBoomPrefabScale = 1;

        /// <summary>
        /// 受击特效路径
        /// </summary>
        [LabelText("受击特效路径")] public string HitPrefabPath;

        /// <summary>
        /// 技能音效路径
        /// </summary>
        [LabelText("技能音效路径")] public string SkillAudioPath;

        /// <summary>
        /// 技能音效播放延迟
        /// </summary>
        [LabelText("技能音效播放延迟")] public float AudioDelay;
    }
}