using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneHumidityMinimum : Ironbug_Component
    {

        private static HVAC.IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneHumidityMinimum()
          : base("Ironbug_SetpointManagerSingleZoneHumidityMinimum", "SPM_SZHumidityMin",
              _fieldSet.OwnerEpNote,
              "Ironbug", "05:SetpointManager")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary| GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControlZone", "_Ctrlzone", "ControlZone", GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSingleZoneHumidityMinimum", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone>();
            DA.GetDataList(0, zones);
            var zone = (HVAC.BaseClass.IB_ThermalZone)null;
            if (zones.Count == 0) return;

            if (zones.Count > 1) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This is the setpointManager for the single zone, you have more than one zone as input. So it takes the first zone as the control zone.");
            zone = zones[0];
            var obj = new HVAC.IB_SetpointManagerSingleZoneHumidityMinimum(zone);

            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointHumidityMin;

        public override Guid ComponentGuid => new Guid("{14B9E046-8B53-4F67-8789-463161DCDAA5}");
    }
}