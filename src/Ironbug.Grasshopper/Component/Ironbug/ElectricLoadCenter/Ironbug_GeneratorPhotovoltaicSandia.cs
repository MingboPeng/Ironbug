using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GeneratorPhotovoltaicSandia : Ironbug_HVACWithParamComponent
    {
        public Ironbug_GeneratorPhotovoltaicSandia()
          : base("IB_GeneratorPhotovoltaicSandia", "PVSandia",
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
            var obj = new HVAC.IB_GeneratorPhotovoltaicSandia();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("B8C9D0E1-F2A3-4567-8901-CDEF12345678");
    }
}