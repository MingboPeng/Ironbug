using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using System;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GeneratorPhotovoltaic : Ironbug_HVACWithParamComponent
    {
        public Ironbug_GeneratorPhotovoltaic()
          : base("IB_GeneratorPhotovoltaic", "Photovoltaic",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_GeneratorPhotovoltaic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ShadeSurface", "_surface", "A Honeybee Shade", GH_ParamAccess.item);
            pManager[pManager.AddGenericParameter("_performance_", "_performance_", "One of PhotovoltaicPerformances", GH_ParamAccess.item)].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Generator", "Generator", "Generator", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GeneratorPhotovoltaic();

            var surface = (object)null;
            var perf = (IB_PhotovoltaicPerformance)null;

            if (DA.GetData(0, ref surface))
            {
                var shadeID = Helper.GetShadeName(surface);
                obj.SetSurface(shadeID);
            }

            if (DA.GetData(1, ref perf))
            {
                obj.SetPhotovoltaicPerformance(perf);
            }


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("C9D0E1F2-A3B4-5678-9012-DEF123456789");
    }
}