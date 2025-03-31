using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SolarCollectorPerformanceFlatPlate : Ironbug_HVACWithParamComponent
    {
        public Ironbug_SolarCollectorPerformanceFlatPlate()
          : base("IB_SolarCollectorPerformanceFlatPlate", "SCFP_Performance",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_SolarCollectorPerformanceFlatPlate_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("IB_SolarCollectorPerformanceFlatPlate", "SCFP_Performance", "Connect to IB_SolarCollectorFlatPlateWater", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SolarCollectorPerformanceFlatPlate();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("96184A20-A628-4DB6-A35E-B3B60E3B2A9D");

    }

}