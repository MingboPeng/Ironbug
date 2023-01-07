using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACPackagedTerminalHeatPump : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ZoneHVACPackagedTerminalHeatPump()
          : base("IB_ZoneHVACPackagedTerminalHeatPump", "PT HeatPump",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACPackagedTerminalHeatPump_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PTHP;

        public override Guid ComponentGuid => new Guid("C58E770A-B9F1-4D17-88C0-0AEA2518731C");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide heating source. CoilHeatingDX", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. CoilCoolingDX", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "FanConstantVolume, FanVariableVolume, or FanOnOff", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SupplementalHeatingCoil", "supCoilH_", "Supplemental heating coil: CoilHeatingElectric", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACPackagedTerminalAirConditioner", "PTHP", "Connect to zone's equipment", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            HVAC.BaseClass.IB_Fan fan = new IB_FanConstantVolume();
            var coilH = new IB_CoilHeatingDXSingleSpeed();
            var coilC = new IB_CoilCoolingDXSingleSpeed();
            var spCoilH = new IB_CoilHeatingElectric();

            DA.GetData(0, ref coilH);
            DA.GetData(1, ref coilC);
            DA.GetData(2, ref fan);
            DA.GetData(3, ref spCoilH);

            var obj = new HVAC.IB_ZoneHVACPackagedTerminalHeatPump(fan,coilH,coilC,spCoilH);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
    }
}