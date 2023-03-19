using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerNightVentilation : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerNightVentilation()
          : base("IB_AvailabilityManagerNightVentilation", "AM_NightVent",
                 "Description",
                 "Ironbug", "05:SetpointManager", typeof(IB_AvailabilityManagerNightVentilation_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("controlZone", "_ctrlZone", "A controlled zone served by the air loop defined. The air conditions in this zone are used to determine if natural ventilation should be provided.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AvailabilityManager", "AM", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object zone = null;
            if (!DA.GetData(0, ref zone)) return;

            var zoneName = Helper.GetRoomName(zone);
            var obj = new IB_AvailabilityManagerNightVentilation();
            obj.SetControlZone(zoneName);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("4506EFDA-791D-46A6-BBAB-4D7E70ACB457");
    }
}