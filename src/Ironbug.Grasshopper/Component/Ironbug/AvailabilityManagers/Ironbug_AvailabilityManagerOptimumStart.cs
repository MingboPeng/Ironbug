﻿using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerOptimumStart : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerOptimumStart()
          : base("IB_AvailabilityManagerOptimumStart", "AM_OptimumStart",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_AvailabilityManagerOptimumStart_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("controlZone", "_ctrlZone", "A controlled zone served by the air loop defined. The air conditions in this zone are used to determine if natural ventilation should be provided.", GH_ParamAccess.item);
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
            var obj = new IB_AvailabilityManagerOptimumStart();
            obj.SetControlZone(zoneName);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AM_Opt;

        public override Guid ComponentGuid => new Guid("FF1B3CB5-54F7-4EB7-BE62-4DFC0BFCB6ED");
    }
}