using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerScheduled : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerScheduled()
          : base("IB_AvailabilityManagerScheduled", "AM_NightCycle",
                 "Description",
                 "Ironbug", "05:SetpointManager", typeof(IB_AvailabilityManagerScheduled_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AvailabilityManager", "AM", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AvailabilityManagerScheduled();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("3A1AE4A9-ECE5-4F8F-A21C-B35B48E3F40E");
    }
}