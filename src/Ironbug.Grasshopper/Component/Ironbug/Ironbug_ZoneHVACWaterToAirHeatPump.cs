using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACWaterToAirHeatPump : Ironbug_HVACComponent
    {
        public Ironbug_ZoneHVACWaterToAirHeatPump()
          : base("Ironbug_ZoneHVACWaterToAirHeatPump", "WaterAirHeatPump",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACPackagedTerminalHeatPump_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterToAirHeatPump;

        public override Guid ComponentGuid => new Guid("F35104CC-614A-4F4D-81AC-236C710E37E2");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide heating source. use CoilHeatingWaterToAirHeatPump", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoolingCoil", "coilC_", "Cooling coil to provide cooling source. use CoilCoolingWaterToAirHeatPump", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Fan", "fan_", "Can be FanOnOff", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SupplementalHeatingCoil", "spCoilH_", "Backup HeatingCoil. CoilHeatingElectric, CoilHeatingGas, or CoilHeatingWater", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACWaterToAirHeatPump", "PTHP", "Connect to zone's equipment", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var fan = new IB_FanConstantVolume();
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