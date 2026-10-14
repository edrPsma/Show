using System.Collections.Generic;
using Guild;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.GuildHeartLaw)]
    public class GuildHeartLawCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var model = this.GetModel<IGuildModel>().GuildHeartLawModel;
            foreach (var data in model.datas.Values)
            {
                if (!data.isActive) continue;

                DTGuild_heart_law cfg = DataTable.GetItem<DTGuild_heart_law>(data.id);
                if (cfg != null)
                {
                    foreach (var attr in cfg.Type1)
                    {
                        int attrID = attr[0];
                        int attrVal = attr[1];
                        PropertyInfo info = list.Find(d => d.ID == attrID);
                        if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                        else info.Value += attrVal;
                    }
                }
            }

        }
    }
}