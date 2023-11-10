using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerSingleZoneCooling : Ironbug_HVACWithParamComponent
    {

        private static HVAC.IB_SetpointManagerSingleZoneCooling_FieldSet _fieldSet = HVAC.IB_SetpointManagerSingleZoneCooling_FieldSet.Value;
        public Ironbug_SetpointManagerSingleZoneCooling()
          : base("IB_SetpointManagerSingleZoneCooling", "SPM_SZCooling",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerSingleZoneCooling_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControlZone", "_Ctrlzone", EPDoc.SetpointManagerSingleZoneCooling.Field_ControlZoneName, GH_ParamAccess.list);
            pManager[0].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerSingleZoneCooling", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<HVAC.BaseClass.IB_ThermalZone> ();
                
            if (!DA.GetDataList(0, zones) || zones?.FirstOrDefault() == null)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid control zone.");
                return;
            }
            var zone = zones.FirstOrDefault();
            var zoneName = Helper.GetRoomName(zone);
            var obj = new HVAC.IB_SetpointManagerSingleZoneCooling();
            obj.SetControlZone(zoneName);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointSZCooling;

        public override Guid ComponentGuid => new Guid("{75BDBFF8-2A08-41D4-9D3B-0407F2E789B5}");
    }
}