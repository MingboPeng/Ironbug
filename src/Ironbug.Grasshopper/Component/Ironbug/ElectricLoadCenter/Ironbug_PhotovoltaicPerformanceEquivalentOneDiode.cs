using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PhotovoltaicPerformanceEquivalentOneDiode : Ironbug_HVACWithParamComponent
    {
        public Ironbug_PhotovoltaicPerformanceEquivalentOneDiode()
          : base("IB_PhotovoltaicPerformanceEquivalentOneDiode", "PvPerfSDM",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_PhotovoltaicPerformanceEquivalentOneDiode_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PhotovoltaicPerformanceEquivalentOneDiode", "pvPerf", "Connect to IB_GeneratorPhotovoltaic", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PhotovoltaicPerformanceEquivalentOneDiode();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PVPerfSDM;

        public override Guid ComponentGuid => new Guid("176AFE0D-7448-4F0B-891D-247C088FCA4A");
    }
}