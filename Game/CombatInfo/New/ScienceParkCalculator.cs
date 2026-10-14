using System.Collections.Generic;
using Homeland;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.SciencePark)]
    public class ScienceParkCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var model = this.GetModel<IHomelandModel>().scienceParkModel;
            foreach (var scienceGroup in model.scienceGroups.Values)
            {
                var data = scienceGroup.activeScience;
                if (data == null) continue;

                var property = data.Type1;
                if (property == null || property.Count != 2) continue;

                int attrID = property[0];
                int attrVal = property[1];
                PropertyInfo info = list.Find(d => d.ID == attrID);
                if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                else info.Value += attrVal;
            }
        }
    }
}