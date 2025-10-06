using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;
using System;

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
            pManager.AddGenericParameter("ShadeSurface", "_surface", "A Honeybee Shade", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("generator", "generator", "generator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GeneratorPVWatts();
            var surface = (object)null;
            if (DA.GetData(0, ref surface))
            {
                var shadeID = Helper.GetShadeName(surface);
                obj.SetSurface(shadeID);
            }

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PVWatts;

        public override Guid ComponentGuid => new Guid("D0E1F2A3-B4C5-6789-0123-EF1234567890");
    }
}