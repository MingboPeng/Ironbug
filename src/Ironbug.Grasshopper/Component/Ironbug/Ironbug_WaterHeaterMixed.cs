using System;
using Grasshopper.Kernel;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterMixed : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_WaterHeaterMixed()
          : base("IB_WaterHeaterMixed", "WaterHeaterMixed",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_WaterHeaterMixed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("AmbientTemperatureThermalZone", "AmbientTemperatureThermalZone_", "", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterHeaterMixed", "WaterHeater", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_ThermalZone zone = null;
            var obj = new HVAC.IB_WaterHeaterMixed();
            if (DA.GetData(0, ref zone))
            {
                obj = new HVAC.IB_WaterHeaterMixed(zone);
            }
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;
        
        public override Guid ComponentGuid => new Guid("A202ED17-8359-4A2A-A0DD-D17515DCC5CF");
    }
}