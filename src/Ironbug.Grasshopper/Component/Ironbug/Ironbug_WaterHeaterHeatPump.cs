using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterHeatPump : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_WaterHeaterHeatPump()
          : base("IB_WaterHeaterHeatPump", "WaterHeaterHeatPump",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_WaterHeaterHeatPump_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;

        public override Guid ComponentGuid => new Guid("61ACA84B-DAAF-4ECE-8271-5796FF8C3A0D");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // I believe all of these parameters are necessary, but I'm not sure whether I need to explicitly add ALL of the parameters listed in
            // https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-025.html#waterheaterheatpump -- it seems that the Ironbug_DuplicableHVACWithParamComponent class takes care of that
            pManager.AddGenericParameter("Water Heater Mixed", "waterHeater_", "Water Heater Mixed. use WaterHeaterMixed", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatingCoil", "coilH_", "Heating coil to provide heating source..", GH_ParamAccess.item);
            pManager.AddGenericParameter("Fan", "fan_", "Supply fan. By default, no fan is included.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterHeaterHeatPump", "WaterHeaterHeatPump", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // HVAC.BaseClass.IB_Fan fan = null;
            // HVAC.BaseClass.IB_WaterHeaterMixed waterHeater = null;
            // HVAC.BaseClass.IB_CoilDX coilH = null;
            // HVAC.BaseClass.Fan fan = null;

            var obj = new HVAC.IB_WaterHeaterHeatPump();


            var waterHeater = (IB_WaterHeaterMixed)null;
            var coilH = (IB_CoilDX)null;
            var fan = (IB_Fan)null;
            

            if (DA.GetData(0, ref waterHeater))
            {
                obj.SetTank(waterHeater);
            }

            if (DA.GetData(1, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }

            if (DA.GetData(2, ref fan))
            {
                obj.SetFan(fan);
            }


            this.SetObjParamsTo(obj);
            
            // current:
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

            // or is this supposed to be:
            // DA.SetData(0, obj)
        }

    }
}