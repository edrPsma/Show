using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Badge)]
    public class BadgeCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var badgeModel = this.GetModel<IBadegeModel>();
            var atts = badgeModel.CalculateAttribute(badgeModel.CurBadgeLevelData.career, badgeModel.CurBadgeLevelData.level);

            foreach (var att in atts)
            {
                int key = att.Key;
                long value = att.Value;

                list.Add(new PropertyInfo(key, value));
            }
        }
    }
}