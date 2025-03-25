using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_PlantEquipmentOperationHeatingLoad : IB_PlantEquipmentOperationSchemeBase
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantEquipmentOperationHeatingLoad();

        private static PlantEquipmentOperationHeatingLoad NewDefaultOpsObj(Model model) => new PlantEquipmentOperationHeatingLoad(model);

        public IB_PlantEquipmentOperationHeatingLoad() : base(NewDefaultOpsObj)
        {
        }
    
        public override ModelObject ToOS(Model model, PlantLoop loop)
        {
            var htg_op_scheme = NewDefaultOpsObj(model);

            foreach (var item in this._equipments)
            {
                var obj = item.obj.GetOsmObjInModel(model);
                if (obj == null)
                    continue;
                var hvacObj = obj.to_HVACComponent();
                if (hvacObj.isNull())
                    continue;

                htg_op_scheme.addEquipment(item.limit, hvacObj.get());
            }

            loop.setPlantEquipmentOperationHeatingLoad(htg_op_scheme);
            return htg_op_scheme;
        }
        
    }

}