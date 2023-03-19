using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerScheduledOff : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerScheduledOff()
          : base("IB_AvailabilityManagerScheduledOff", "AM_NightCycle",
                 "Description",
                 "Ironbug", "05:SetpointManager", typeof(IB_AvailabilityManagerScheduledOff_FieldSet))
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
            var obj = new IB_AvailabilityManagerScheduledOff();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("446B0555-E983-4FA7-9E09-DB126332AFD2");
    }
}