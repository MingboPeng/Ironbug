using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneHeating : Ironbug_HVACWithParamComponent
    {

        private static HVAC.IB_SetpointManagerSingleZoneHeating_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneHeating_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneHeating()
          : base("Ironbug_SetpointManagerSingleZoneHeating", "SPM_SZCooling",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerSingleZoneHeating_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControlZone", "_Ctrlzone", EPDoc.SetpointManagerSingleZoneHeating.Field_ControlZoneName, GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSingleZoneHeating", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone>();
            DA.GetDataList(0, zones);
            var zone = (HVAC.BaseClass.IB_ThermalZone)null;
            if (zones.Count == 0) return;

            if (zones.Count > 1) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This is a setpointManager for the single zone, you have more than one zone as input. So it takes the first zone as the control zone.");
            zone = zones[0];
            var obj = new HVAC.IB_SetpointManagerSingleZoneHeating(zone);


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointSZHeaing;

        public override Guid ComponentGuid => new Guid("{2098A2AD-DA1E-48A2-9738-28E91B2BCBFD}");
    }
}