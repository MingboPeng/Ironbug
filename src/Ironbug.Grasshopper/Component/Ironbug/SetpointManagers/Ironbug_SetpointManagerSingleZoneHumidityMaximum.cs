using Grasshopper.Kernel;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneHumidityMaximum : Ironbug_HVACComponent
    {

        //private static HVAC.IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneHumidityMaximum()
          : base("IB_SetpointManagerSingleZoneHumidityMaximum", "SPM_SZHumidityMax",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet))
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
            pManager.AddGenericParameter("SetpointManagerSingleZoneHumidityMaximum", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone>();
            DA.GetDataList(0, zones);
            var zone = (HVAC.BaseClass.IB_ThermalZone)null;
            if (zones.Count == 0) return;

            if (zones.Count > 1) this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This is the setpointManager for the single zone, you have more than one zone as input. So it takes the first zone as the control zone.");
            zone = zones[0];
            var obj = new HVAC.IB_SetpointManagerSingleZoneHumidityMaximum(zone);


            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointHumidityMax;

        public override Guid ComponentGuid => new Guid("{419E4137-B5E0-4A78-9FB3-41119A0624EC}");
    }
}