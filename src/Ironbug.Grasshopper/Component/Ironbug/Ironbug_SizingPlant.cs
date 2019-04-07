using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SizingPlant : Ironbug_HVACComponent
    {
       
        public Ironbug_SizingPlant()
          : base("Ironbug_SizingPlant", "SizingPlant",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_SizingPlant_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SizingPlant", "Sz", "SizingPlant", GH_ParamAccess.item);
        }
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SizingPlant();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SizingPlant;
        
        public override Guid ComponentGuid => new Guid("48A8B6F8-4AAF-44BD-8923-979370F36817");
    }
}