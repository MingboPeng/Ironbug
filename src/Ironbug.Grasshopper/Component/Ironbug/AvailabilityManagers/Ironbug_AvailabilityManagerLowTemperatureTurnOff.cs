using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerLowTemperatureTurnOff : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerLowTemperatureTurnOff()
          : base("IB_AvailabilityManagerLowTemperatureTurnOff", "AM_LowOff",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_AvailabilityManagerLowTemperatureTurnOff_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Sensor Probe", "_probe", "Add a IB_NodeProbe to a loop first, and then connect the probe to here for availability manager to use.", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AvailabilityManager", "AM", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_NodeProbe probe = null;
            if (!DA.GetData(0, ref probe)) return;

            var nodeID = probe.GetTrackingID();
            var obj = new IB_AvailabilityManagerLowTemperatureTurnOff();
            obj.SetSensorNode(nodeID);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AM_Off;

        public override Guid ComponentGuid => new Guid("D5717D6F-B3B3-4AD5-B32C-2F8460AC76F0");
    }
}