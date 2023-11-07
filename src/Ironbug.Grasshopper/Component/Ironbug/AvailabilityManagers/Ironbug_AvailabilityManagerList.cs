using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Ironbug_AvailabilityManagerList : Ironbug_Component
    {

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public Ironbug_Ironbug_AvailabilityManagerList()
          : base("IB_AvailabilityManagerList", "AMList",
              "Use AvailabilityManagerList if you have more than one availability managers (from highest precedence to lowest) for one loop",
               "Ironbug", "05:SetpointManager & AvailabilityManager")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Availability managers", "managers", "A list of availability managers (from highest precedence to lowest) that will be grouped.", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AvailabilityManagerList", "AMList", "A list of availability managers that will be added to each loop. Connect to loop's AvailabilityManager", GH_ParamAccess.item);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var managers = new List<IB_AvailabilityManager>();
            DA.GetDataList(0, managers);

            var group = new IB_AvailabilityManagerList();
            group.SetManagers(managers);
            DA.SetData(0, group);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AM_Group;

        public override Guid ComponentGuid => new Guid("6E86EBFB-28E6-45F7-BDB4-7FDA4D560354");
    }


}