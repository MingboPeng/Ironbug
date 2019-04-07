using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACUnitVentilator_Cooling : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        
        public Ironbug_ZoneHVACUnitVentilator_Cooling()
          : base("Ironbug_ZoneHVACUnitVentilator_Cooling", "UnitVentCooling",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACUnitVentilator_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. can be CoilHeatingWater, CoilHeatingElectirc, or CoilHeatingGas.", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanConstantVolume or FanVariableVolume.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACUnitVentilator_Cooling", "UnitVent", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACUnitVentilator_CoolingOnly();
            

            var fan = (IB_Fan)null;
            var coilC = (IB_CoilCoolingBasic)null;
            

            if (DA.GetData(0, ref coilC))
            {
                obj.SetCoolingCoil(coilC);
            }

            if (DA.GetData(1, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.UnitVentC;

        public override Guid ComponentGuid => new Guid("F9A4E1F5-4C8D-4E17-8025-DBA7F329D479");
    }
}