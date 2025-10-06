using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PhotovoltaicPerformanceSandia : Ironbug_HVACWithParamComponent
    {
        public Ironbug_PhotovoltaicPerformanceSandia()
          : base("IB_PhotovoltaicPerformanceSandia", "PvPerfSandia",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_PhotovoltaicPerformanceSandia_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PhotovoltaicPerformanceSandia", "pvPerf", "Connect to IB_GeneratorPhotovoltaic", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PhotovoltaicPerformanceSandia();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PVPerfSandia;

        public override Guid ComponentGuid => new Guid("A979F80F-7F15-4A82-A4F4-A6D29BBC828B");
    }
}