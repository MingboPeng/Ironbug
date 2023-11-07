using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.AvailabilityManager;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AvailabilityManagerHighTemperatureTurnOn : Ironbug_HVACWithParamComponent
    {
        public Ironbug_AvailabilityManagerHighTemperatureTurnOn()
          : base("IB_AvailabilityManagerHighTemperatureTurnOn", "AM_HighOn",
                 "Description",
                 "Ironbug", "05:SetpointManager & AvailabilityManager", typeof(IB_AvailabilityManagerHighTemperatureTurnOn_FieldSet))
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
            var obj = new IB_AvailabilityManagerHighTemperatureTurnOn();
            obj.SetSensorNode(nodeID);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.AM_On;

        public override Guid ComponentGuid => new Guid("6FEFF3D5-F37B-4989-8FC9-16545DADA803");
    }
}