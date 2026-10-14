using System.Collections.Generic;
using Homeland;

namespace Combat.CombatInformation
{
    [Calculator(ECalculatorType.PetFormation)]
    public class PetFormationCalculator : BaseCombatInfoCalculator
    {
        public override void CalculatingLocalProperty(List<PropertyInfo> list)
        {
            var model = this.GetModel<IHomelandModel>().petFormationModel;

            if (model.PetFormations == null) return;

            foreach (var petFormation in model.PetFormations)
            {
                int len = petFormation.Holes.Count;
                bool isAllGoBattle = true;

                for (int i = 0; i < len; i++)
                {
                    var hole = petFormation.Holes[i];
                    if (!hole.IsActive || hole.PetID == default)
                    {
                        isAllGoBattle = false;
                        continue;
                    }

                    // 单个阵法孔位属性计算
                    var cfg = DataTable.GetItem<DTBattle_formation_hole>(hole.ID);
                    if (cfg != null)
                    {
                        int attrID = cfg.Attribute[0];
                        int attrVal = cfg.Attribute[1];
                        PropertyInfo info = list.Find(d => d.ID == attrID);
                        if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                        else info.Value += attrVal;
                    }
                }

                // 全上阵额外属性计算
                if (isAllGoBattle)
                {
                    var cfg = DataTable.GetItem<DTBattle_formation>(petFormation.ID);
                    if (cfg != null)
                    {
                        int attrID = cfg.Attribute[0];
                        int attrVal = cfg.Attribute[1];
                        PropertyInfo info = list.Find(d => d.ID == attrID);
                        if (info == null) list.Add(new PropertyInfo(attrID, attrVal));
                        else info.Value += attrVal;
                    }
                }
            }
        }
    }
}