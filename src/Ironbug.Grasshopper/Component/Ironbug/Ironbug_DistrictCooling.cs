using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_DistrictCooling : Ironbug_HVACComponent
    {
        public Ironbug_DistrictCooling()
          : base("Ironbug_DistrictCooling", "DistrictCooling",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_DistrictCooling_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DistrictCooling", "DistCooling", "DistrictCooling for plant loop's supply.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_DistrictCooling();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.DistricCooling;

        public override Guid ComponentGuid => new Guid("96db6f68-01ad-44a9-bb0b-bf30459c8fbe");
    }
}