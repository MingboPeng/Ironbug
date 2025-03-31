using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SolarCollectorFlatPlateWater : Ironbug_HVACWithParamComponent
    {
        public Ironbug_SolarCollectorFlatPlateWater()
          : base("IB_SolarCollectorFlatPlateWater", "SCFP_Water",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_SolarCollectorFlatPlateWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ShadeSurface", "_shade", "Honeybee Shade", GH_ParamAccess.item);
            pManager[pManager.AddGenericParameter("Solar Collector Performance", "sc_performance_", "From IB_SolarCollectorPerformanceFlatPlate", GH_ParamAccess.item)].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("IB_SolarCollectorFlatPlateWater", "SCFP_Water", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SolarCollectorFlatPlateWater();
            var surface = (object)null;
            var sc_p = (IB_SolarCollectorPerformanceFlatPlate)null;

            if (DA.GetData(0, ref surface))
            {
                var shadeID = Helper.GetShadeName(surface);
                obj.SetSurface(shadeID);
            }

            if (DA.GetData(1, ref sc_p))
            {
                obj.SetSolarCollectorPerformance(sc_p);
            }


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("73276C91-C278-4BC3-9AC7-B57FFA5A4522");

    }

}