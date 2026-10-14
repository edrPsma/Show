using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.CombatInformation
{
    public class CalculatorAttribute : ReflectionAttribute
    {
        public CalculatorAttribute(ECalculatorType key) : base(typeof(BaseCombatInfoCalculator), key, 100)
        {
        }
    }
}