using Grasshopper.Kernel;
using System;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirReset_Adv : Ironbug_DuplicableHVACWithParamComponent
    {
        private static HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet _fieldSet = HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet.Value;

        /// Initializes a new instance of the Ironbug_SetpointManagerOutdoorAirReset class.

        public Ironbug_SetpointManagerOutdoorAirReset_Adv()
          : base("IB_SetpointManagerOutdoorAirReset+", "SPM_OAReset+",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerOutdoorAirReset_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerOutdoorAirReset", "SPM", "TODO:...", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirReset();


            this.SetObjParamsTo(obj);
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

        public override Guid ComponentGuid => new Guid("6B8F9683-CBE7-4DC7-982B-36AA6DEA52D3");
    }
}