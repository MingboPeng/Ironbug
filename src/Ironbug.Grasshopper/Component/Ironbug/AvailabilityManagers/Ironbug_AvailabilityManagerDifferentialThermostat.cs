using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerDifferentialThermostat : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerDifferentialThermostat()
          : base("IB_AvailabilityManagerDifferentialThermostat", "AM_Diff",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_AvailabilityManagerDifferentialThermostat_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Cold Probe", "_coldProbe", $"Add a IB_NodeProbe (cold) to a loop first, and then connect the probe to here for availability manager to use.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Hot Probe", "_hotProbe", "Add a IB_NodeProbe (hot) to a loop first, and then connect the probe to here for availability manager to use.", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AvailabilityManager", "AM", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_NodeProbe cold = null;
            IB_NodeProbe hot = null;
            if (!DA.GetData(0, ref cold)) return;
            if (!DA.GetData(0, ref hot)) return;

            var coldID = cold.GetTrackingID();
            var hotID = hot.GetTrackingID();

            var obj = new IB_AvailabilityManagerDifferentialThermostat();
            obj.SetSensorNode(coldID, hotID);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AM_Dif;

        public override Guid ComponentGuid => new Guid("F1E8995C-953F-44BF-88DD-076B1F8CBC73");
    }
}