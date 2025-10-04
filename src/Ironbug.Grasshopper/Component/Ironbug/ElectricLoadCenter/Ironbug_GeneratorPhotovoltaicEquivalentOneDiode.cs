using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GeneratorPhotovoltaicEquivalentOneDiode : Ironbug_HVACWithParamComponent
    {
        public Ironbug_GeneratorPhotovoltaicEquivalentOneDiode()
          : base("IB_GeneratorPhotovoltaicEquivalentOneDiode", "PVEquivalentOneDiode",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
             typeof(HVAC.IB_GeneratorPhotovoltaic_FieldSet))
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
            var obj = new HVAC.IB_GeneratorPhotovoltaicEquivalentOneDiode();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("A7B8C9D0-E1F2-3456-7890-BCDEF1234567");
    }
}