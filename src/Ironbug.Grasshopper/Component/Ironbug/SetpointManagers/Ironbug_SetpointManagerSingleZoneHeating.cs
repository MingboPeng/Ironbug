using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneHeating : Ironbug_HVACWithParamComponent
    {

        private static HVAC.IB_SetpointManagerSingleZoneHeating_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneHeating_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneHeating()
          : base("IB_SetpointManagerSingleZoneHeating", "SPM_SZHeating",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
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
            List<HVAC.BaseClass.IB_ThermalZone> zones = null;

            if (!DA.GetDataList(0, zones) || zones?.FirstOrDefault() == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid control zone.");
                return;
            }
            var zone = zones.FirstOrDefault();

            var obj = new HVAC.IB_SetpointManagerSingleZoneHeating(zone);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointSZHeaing;

        public override Guid ComponentGuid => new Guid("{2098A2AD-DA1E-48A2-9738-28E91B2BCBFD}");
    }
}