using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.AI
{
    /// <summary>
    /// 选择技能行为
    /// </summary>
    public class SelectSkillAction : BaseAction
    {
        public override void OnStart()
        {
            _entity.AnimationController.PlayAnimation(EAnimationName.idle);
            List<SkillInfo> skillInfos = _entity.SkillOwner.SkillInfos;

            bool skillTest = PlayerPrefs.GetInt(PlayerPrefsDefine.SKILL_TEST) == 1;

            if (!skillTest)
            {
                for (int i = 0; i < skillInfos.Count; i++)
                {
                    float curTimer = _entity.SkillOwner.GetSkillTimer(skillInfos[i].Data.ID);
                    if (curTimer > 0) continue;

                    DTSkill_base skillData = skillInfos[i].Data;
                    if (skillData.SkillType == DTSkill_base.ESkillType.Zhi_ye_zhu_dong && _entity.IsHero()) continue;

                    _entity.SkillOwner.ReleaseQueue.AddLast(skillInfos[i]);
                }
            }
        }
    }
}