using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Equip)]
    public class EquipCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var atts = this.GetSystem<IEquipSytem>().AllCalculateAttribute();
            foreach (var att in atts)
            {
                list.Add(new PropertyInfo(att.Key, att.Value));
            }
        }
    }
}