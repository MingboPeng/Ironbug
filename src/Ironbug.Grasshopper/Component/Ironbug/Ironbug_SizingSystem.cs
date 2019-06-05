using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SizingSystem : Ironbug_HVACComponent
    {
        public Ironbug_SizingSystem()
          : base("Ironbug_SizingSystem", "SizingSystem",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_SizingSystem_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SizingSystem", "Sz", "SizingSystem", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SizingSystem();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SizingSys;

        public override Guid ComponentGuid => new Guid("73B5D3F2-9F13-4B2D-917B-E08EAE697124");
    }
}