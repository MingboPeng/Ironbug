using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PlantEquipmentOperationCoolingLoad : IB_PlantEquipmentOperationSchemeBase
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantEquipmentOperationCoolingLoad();

        private static PlantEquipmentOperationCoolingLoad NewDefaultOpsObj(Model model) => new PlantEquipmentOperationCoolingLoad(model);

        public IB_PlantEquipmentOperationCoolingLoad() : base(NewDefaultOpsObj)
        {
        }

        public override ModelObject ToOS(Model model, PlantLoop loop)
        {
            var htg_op_scheme = NewDefaultOpsObj(model);

            foreach (var item in this._equipments)
            {
                var obj = item.Obj.GetOsmObjInModel(model);
                if (obj == null)
                    continue;
                var hvacObj = obj.to_HVACComponent();
                if (hvacObj.isNull())
                    continue;

                htg_op_scheme.addEquipment(item.Limit, hvacObj.get());
            }

            loop.setPlantEquipmentOperationCoolingLoad(htg_op_scheme);
            return htg_op_scheme;
        }

    }

}