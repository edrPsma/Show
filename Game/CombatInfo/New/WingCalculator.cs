using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Wing)]
    public class WingCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var wingModel = this.GetModel<IWingModel>();
            var atts = wingModel.CalculateAttribute(wingModel.UseWingID.Value);
            foreach (var att in atts)
            {
                list.Add(new PropertyInfo(att.Key, att.Value));
            }
        }
    }
}