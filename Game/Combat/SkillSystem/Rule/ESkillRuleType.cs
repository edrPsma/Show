using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Combat
{
#if UNITY_EDITOR
    public class SkillRuleHelper
    {
        public static Dictionary<ESkillRuleType, string> DescDic = new Dictionary<ESkillRuleType, string>
        {
            {ESkillRuleType.FlyingBullets,"参数1:速度"},
            {ESkillRuleType.ProjectBullets,"参数1:速度\n参数2:角度"},
            {ESkillRuleType.Spellcaster,"参数1:数量\n参数2:技能ID\n参数3:释放间隔"},
            {ESkillRuleType.RandomSpellCaster,"参数1:数量\n参数2:技能ID\n参数3:释放间隔"},
            {ESkillRuleType.GoAhead,"参数1:速度\n参数2:移动距离"},
            {ESkillRuleType.GoAheadOnlyOne,"参数1:速度\n参数2:移动距离"},
            {ESkillRuleType.RotateProjectBullets,"参数1:速度\n参数2:角度\n参数3:旋转速度"},
        };
    }
#endif

    public enum ESkillRuleType
    {
        [LabelText("无")]
        None = 0,

        [LabelText("直接飞行物")]
        FlyingBullets = 1,

        [LabelText("投射")]
        ProjectBullets = 2,

        [LabelText("施法")]
        Spellcaster = 3,

        [LabelText("随机目标施法")]
        RandomSpellCaster = 4,

        [LabelText("向前移动")]
        GoAhead = 5,

        [LabelText("穿透子弹")]
        GoAheadOnlyOne = 6,

        [LabelText("旋转投射")]
        RotateProjectBullets = 7,
    }
}