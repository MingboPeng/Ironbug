using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneHumidityMinimum : Ironbug_HVACComponent
    {

        //private static HVAC.IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneHumidityMinimum()
          : base("IB_SetpointManagerSingleZoneHumidityMinimum", "SPM_SZHumidityMin",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary| GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControlZone", "_Ctrlzone", EPDoc.SetpointManagerSingleZoneHumidityMinimum.Field_ControlZoneAirNodeName, GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSingleZoneHumidityMinimum", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone>();

            if (!DA.GetDataList(0, zones) || zones?.FirstOrDefault() == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid control zone.");
                return;
            }
            var zone = zones.FirstOrDefault();
            var obj = new HVAC.IB_SetpointManagerSingleZoneHumidityMinimum(zone);


            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointHumidityMin;

        public override Guid ComponentGuid => new Guid("{14B9E046-8B53-4F67-8789-463161DCDAA5}");
    }
}