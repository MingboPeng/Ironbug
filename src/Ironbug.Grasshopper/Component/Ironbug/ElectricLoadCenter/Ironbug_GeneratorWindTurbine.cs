using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GeneratorWindTurbine : Ironbug_HVACWithParamComponent
    {
        public Ironbug_GeneratorWindTurbine()
          : base("IB_GeneratorWindTurbine", "WindTurbine",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_GeneratorWindTurbine_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Generator", "Generator", "Generator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GeneratorWindTurbine();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("E1F2A3B4-C5D6-7890-1234-F1234567890A");
    }
}