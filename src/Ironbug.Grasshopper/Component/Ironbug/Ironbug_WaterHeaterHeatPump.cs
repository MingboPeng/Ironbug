using System;
using Grasshopper.Kernel;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterHeatPump : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_WaterHeaterPump()
          : base("IB_WaterHeaterHeatPump", "WaterHeaterHeatPump",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_WaterHeaterHeatPump_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;

        public override Guid ComponentGuid => new Guid("A202ED17-8359-4A2A-A0DD-D17515DCC5CF");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Water Heater Mixed", "waterHeater_", "Water Heater Mixed. use WaterHeaterMixed", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide heating source. By default, no heating coil is included.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Fan", "fan_", "Supply fan. By default, no fan is included.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterHeaterHeatPump", "WaterHeaterHeatPump", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_Fan fan = null;
            HVAC.BaseClass.IB_WaterHeaterMixed waterHeater = null;
            HVAC.BaseClass.IB_CoilDX coilH = null;
            HVAC.BaseClass.Fan fan = null;

            var obj = new HVAC.IB_WaterHeaterHeatPump();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

    }
}