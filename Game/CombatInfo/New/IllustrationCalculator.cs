using System;
using System.Collections.Generic;
using Illustrations;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.Illustration)]
    public class IllustrationCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var model = this.GetModel<IIllustrationsModel>();
            foreach (DTHandbook.Etype type in Enum.GetValues(typeof(DTHandbook.Etype)))
            {
                var datas = model.GetDatas(type);
                for (int i = 0; i < datas.Count; i++)
                {
                    IllustrationsData data = datas[i];
                    DTHandbook cfg = DataTable.GetItem<DTHandbook>(data.currentID);
                    if (cfg == null) continue;

                    int attrID = cfg.attribute[0];
                    int attrVal = cfg.attribute[1];
                    PropertyInfo info = list.Find(d => d.ID == attrID);
                    if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                    else info.Value += attrVal;
                }
            }
        }
    }
}