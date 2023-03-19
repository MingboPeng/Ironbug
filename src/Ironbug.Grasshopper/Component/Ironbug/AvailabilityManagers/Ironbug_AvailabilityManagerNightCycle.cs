using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerNightCycle : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerNightCycle()
          : base("IB_AvailabilityManagerNightCycle", "AM_NightCycle",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_AvailabilityManagerNightCycle_FieldSet))
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
            var obj = new IB_AvailabilityManagerNightCycle();
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("100C4237-568B-4E26-BB26-FCDCF4CABF2B");
    }
}