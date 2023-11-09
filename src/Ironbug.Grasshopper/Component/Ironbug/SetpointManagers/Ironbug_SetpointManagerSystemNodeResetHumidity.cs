using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SetpointManagerSystemNodeResetHumidity : Ironbug_HVACWithParamComponent
    {
        public Ironbug_SetpointManagerSystemNodeResetHumidity()
          : base("IB_SetpointManagerSystemNodeResetHumidity", "SPM_RestHumidity",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_SetpointManagerSystemNodeResetHumidity_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sensor Probe", "_probe", "Add a IB_NodeProbe to a loop first, and then connect the probe to here for setpoint manager to use.", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSystemNodeResetHumidity", "SPM", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_NodeProbe probe = null;
            if (!DA.GetData(0, ref probe)) return;

            var nodeID = probe.GetTrackingID();
            var obj = new IB_SetpointManagerSystemNodeResetHumidity();
            obj.SetSensorNode(nodeID);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SPM_NodeHumidity;

        public override Guid ComponentGuid => new Guid("{E7111E46-447A-4379-9585-269109D344D0}");
    }
}