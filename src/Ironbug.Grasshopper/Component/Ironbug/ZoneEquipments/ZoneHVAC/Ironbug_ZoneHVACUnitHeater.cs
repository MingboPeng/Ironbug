using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACUnitHeater : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ZoneHVACUnitHeater()
          : base("IB_ZoneHVACUnitHeater", "UnitHeater",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACUnitHeater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coil_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACUnitHeater", "UnitHeater", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACUnitHeater();
            

            var fan = (IB_Fan)null;
            var coil = (IB_CoilHeatingBasic)null;

            if (DA.GetData(0, ref coil))
            {
                obj.SetHeatingCoil(coil);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitHeater;

        public override Guid ComponentGuid => new Guid("89682f80-546f-40c0-8b27-c0a8fea7b351");
    }
}