using Grasshopper.Kernel;
using System;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirReset : Ironbug_DuplicableHVACComponent
    {
        private static HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet _fieldSet = HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet.Value;
        
        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.
        
        public Ironbug_SetpointManagerOutdoorAirReset()
          : base("IB_SetpointManagerOutdoorAirReset", "SPM_OAReset",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("SetpointatOutdoorHighTemperature", "_SpOHTemp", _fieldSet.SetpointatOutdoorHighTemperature.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("OutdoorHighTemperature", "_OHTemp", _fieldSet.OutdoorHighTemperature.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("SetpointatOutdoorLowTemperature", "_SpOLTemp", _fieldSet.SetpointatOutdoorLowTemperature.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("OutdoorLowTemperature", "_OLTemp", _fieldSet.OutdoorLowTemperature.Description, GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerOutdoorAirReset", "SPM", "TODO:...", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirReset();

            double lowT = 0;
            double highT = 0;
            double lowOT = 0;
            double highOT = 0;
            
            DA.GetData(0, ref highT);
            DA.GetData(1, ref highOT);
            DA.GetData(2, ref lowT);
            DA.GetData(3, ref lowOT);
            
            
            obj.SetFieldValue(_fieldSet.SetpointatOutdoorHighTemperature, highT);
            obj.SetFieldValue(_fieldSet.OutdoorHighTemperature, highOT);
            obj.SetFieldValue(_fieldSet.SetpointatOutdoorLowTemperature, lowT);
            obj.SetFieldValue(_fieldSet.OutdoorLowTemperature, lowOT);


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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetpointOAReset;

        public override Guid ComponentGuid => new Guid("f251c255-bb89-4a16-8339-d7adbbdc474a");
    }
}