using Grasshopper.Kernel;
using System;
using System.Linq;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduledDualSetpoint_Adv : Ironbug_DuplicableHVACWithParamComponent
    {
        //private static HVAC.IB_SetpointManagerScheduledDualSetpoint_Adv_FieldSet _fieldSet = HVAC.IB_SetpointManagerScheduledDualSetpoint_Adv_FieldSet.Value;
        public Ironbug_SetpointManagerScheduledDualSetpoint_Adv()
          : base("IB_SetpointManagerScheduledDualSetpoint+", "SPM_Dual+",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerScheduledDualSetpoint_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduledDualSetpoint", "SPM", "TODO:...", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var obj = new HVAC.IB_SetpointManagerScheduledDualSetpoint();


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
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointDualScheduled;

        public override Guid ComponentGuid => new Guid("{D88478BD-AE89-4D6E-B2C4-E67F504B5656}");
    }
}