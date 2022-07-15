using System;
using Grasshopper.Kernel;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PhotovoltaicPerformanceSimple : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_PhotovoltaicPerformanceSimple()
          : base("IB_WaterHeaterMixed", "WaterHeaterMixed",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PhotovoltaicPerformanceSimple_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PhotovoltaicPerformanceSimple", "PhotovoltaicPerformanceSimple", "Connect to PhotovoltaicPerformance", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PhotovoltaicPerformanceSimple();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;
        
        public override Guid ComponentGuid => new Guid("A202ED17-8359-4A2A-A0DD-D17515DCC5CF");
    }
}