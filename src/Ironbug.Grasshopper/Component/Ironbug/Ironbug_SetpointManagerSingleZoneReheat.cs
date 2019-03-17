using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneReheat : Ironbug_Component
    {
        public Ironbug_SetpointManagerSingleZoneReheat()
          : base("Ironbug_SetpointManagerSingleZoneReheat", "SPM_SZReheat",
              EPDoc.SetpointManagerSingleZoneReheat.Note,
              "Ironbug", "05:SetpointManager")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControlZone", "_zone", EPDoc.SetpointManagerSingleZoneReheat.Field_ControlZoneName, GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager.AddNumberParameter("minTemperature", "_minT_", EPDoc.SetpointManagerSingleZoneReheat.Field_MinimumSupplyAirTemperature, GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("maxTemperature", "_maxT_", EPDoc.SetpointManagerSingleZoneReheat.Field_MaximumSupplyAirTemperature, GH_ParamAccess.item, 50);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSingleZoneReheat", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone>();
            DA.GetDataList(0, zones);
            var zone = (HVAC.BaseClass.IB_ThermalZone)null;
            if (zones.Count == 0) return;

            if (zones.Count > 1) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This is the setpointManager for single zone, you have more than one zone as input. So it takes the first zone as the control zone.");
            zone = zones[0];
            var obj = new HVAC.IB_SetpointManagerSingleZoneReheat(zone);
            double minT = 10;
            double maxT = 50;

            var fieldSet = HVAC.IB_SetpointManagerSingleZoneReheat_DataFieldSet.Value;

            if (DA.GetData(1, ref minT)) obj.SetFieldValue(fieldSet.MinimumSupplyAirTemperature, minT);
            if (DA.GetData(2, ref maxT)) obj.SetFieldValue(fieldSet.MaximumSupplyAirTemperature, maxT);

            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointSZReheat;

        public override Guid ComponentGuid => new Guid("DE6F5B3A-0F30-4205-B872-22698641AB32");
    }
}