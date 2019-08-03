using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SizingZone : Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_SizingZone()
          : base("Ironbug_SizingZone", "SizingZone",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_SizingZone_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SizingZone", "Sz", "SizingZone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SizingZone();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SizingZone;

        public override Guid ComponentGuid => new Guid("555eb7a9-bef9-48c8-abe7-32490f2d9aab");
    }
}