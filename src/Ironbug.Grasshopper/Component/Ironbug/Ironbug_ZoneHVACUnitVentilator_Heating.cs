using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACUnitVentilator_Heating : Ironbug_HVACComponent
    {
        public Ironbug_ZoneHVACUnitVentilator_Heating()
          : base("Ironbug_ZoneHVACUnitVentilator_Heating", "UnitVentHeating",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACUnitVentilator_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide reheat source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACUnitVentilator_Heating", "UnitVent", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACUnitVentilator_HeatingOnly();
            

            var fan = (IB_Fan)null;
            var coilH = (IB_CoilHeatingBasic)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan(fan);
            }


            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitVentH;
        public override Guid ComponentGuid => new Guid("2BE94C92-C741-4B0D-8DC3-220224B7D077");
    }
}