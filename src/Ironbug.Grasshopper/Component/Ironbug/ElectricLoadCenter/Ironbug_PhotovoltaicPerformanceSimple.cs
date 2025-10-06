using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PhotovoltaicPerformanceSimple : Ironbug_HVACWithParamComponent
    {
        public Ironbug_PhotovoltaicPerformanceSimple()
          : base("IB_PhotovoltaicPerformanceSimple", "PvPerfSimple",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_PhotovoltaicPerformanceSimple_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PhotovoltaicPerformanceSimple", "pvPerf", "Connect to IB_GeneratorPhotovoltaic", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PhotovoltaicPerformanceSimple();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PVPerfSimple;

        public override Guid ComponentGuid => new Guid("AB4E0B97-78C3-4756-9285-D59F5DDF71A4");
    }
}