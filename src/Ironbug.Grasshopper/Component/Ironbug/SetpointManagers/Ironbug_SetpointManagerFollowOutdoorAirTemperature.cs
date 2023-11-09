using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerFollowOutdoorAirTemperature : Ironbug_DuplicableHVACComponent
    {
        private static HVAC.IB_SetpointManagerFollowOutdoorAirTemperature_FieldSet _fieldSet = HVAC.IB_SetpointManagerFollowOutdoorAirTemperature_FieldSet.Value;
        
        public Ironbug_SetpointManagerFollowOutdoorAirTemperature()
          : base("IB_SetpointManagerFollowOutdoorAirTemperature", "SPM_OATemp",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerFollowOutdoorAirTemperature_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary ;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("ControlVariable", "_CtrlVar_", $"{_fieldSet.ControlVariable.Description} \r\nDefault:Temperature", GH_ParamAccess.item);
            pManager.AddTextParameter("ReferenceTemperatureType", "_RefType_", $"{_fieldSet.ReferenceTemperatureType.Description} \r\nDefault:OutdoorAirWetBulb", GH_ParamAccess.item);
            pManager.AddNumberParameter("MaximumSetpointTemperature", "_maxT_", $"{_fieldSet.MaximumSetpointTemperature.Description}\r\nDefault:80C", GH_ParamAccess.item);
            pManager.AddNumberParameter("MinimumSetpointTemperature", "_minT_", $"{_fieldSet.MinimumSetpointTemperature.Description}\r\nDefault:5C", GH_ParamAccess.item);
            pManager.AddNumberParameter("OffsetTemperatureDifference", "_diff_", $"{_fieldSet.OffsetTemperatureDifference.Description}Default:0", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerFollowOutdoorAirTemperature", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerFollowOutdoorAirTemperature();

            string ctrlVar = "Temperature";
            string refType = "OutdoorAirWetBulb";
            double maxT = 80;
            double minT = 5;
            double diff = 0;
            
            DA.GetData(0, ref ctrlVar);
            DA.GetData(1, ref refType);
            DA.GetData(2, ref maxT);
            DA.GetData(3, ref minT);
            DA.GetData(4, ref diff);
            

            obj.SetFieldValue(_fieldSet.ControlVariable, ctrlVar);
            obj.SetFieldValue(_fieldSet.ReferenceTemperatureType, refType);
            obj.SetFieldValue(_fieldSet.MaximumSetpointTemperature, maxT);
            obj.SetFieldValue(_fieldSet.MinimumSetpointTemperature, minT);
            obj.SetFieldValue(_fieldSet.OffsetTemperatureDifference, diff);


            var objs = this.SetObjDupParamsTo(obj);
            if (objs.Count() == 1)
            {
                DA.SetData(0, obj);
            }
            else
            {
                DA.SetDataList(0, objs);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointFlowOA;

        public override Guid ComponentGuid => new Guid("FF3EEF96-60FF-4D6E-B29D-204D16AF6DBD");
    }
}