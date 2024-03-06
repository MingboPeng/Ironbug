using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SwimmingPoolIndoor : Ironbug_HVACWithParamComponent
    {
        public Ironbug_SwimmingPoolIndoor()
          : base("IB_SwimmingPoolIndoor", "SwimmingPool",
                 "Description",
                 "Ironbug", "02:LoopComponents", typeof(IB_SwimmingPoolIndoor_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("poolSurfaceID", "_poolSrfId", "This is the name of the surface (floor) where the pool is located. Pools are not allowed on any surfaces other than a floor.", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SwimmingPoolIndoor", "Pool", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string srfId = string.Empty;
            if (!DA.GetData(0, ref srfId)) return;

            var obj = new IB_SwimmingPoolIndoor();
            obj.SetWaterSufaceID(srfId);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Resources.SwimmingPool;

        public override Guid ComponentGuid => new Guid("{68E45483-E96D-4864-BB3E-A763DA14143A}");
    }
}