using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GeneratorPVWatts : Ironbug_HVACWithParamComponent
    {
        public Ironbug_GeneratorPVWatts()
          : base("IB_GeneratorPVWatts", "PVWatts",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_GeneratorPVWatts_FieldSet))
        {

        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("SurfaceID", "SurfaceID", "The name of a Surface or ShadingSurface object.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Generator", "Generator", "Generator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GeneratorPVWatts();
            string surfaceId = string.Empty;
            DA.GetData(0, ref surfaceId);
            obj.SetSurface(surfaceId);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("D0E1F2A3-B4C5-6789-0123-EF1234567890");
    }
}