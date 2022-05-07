using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduled_Adv : Ironbug_DuplicableHVACWithParamComponent
    {
        //private static HVAC.IB_SetpointManagerScheduled_FieldSet _fieldSet = HVAC.IB_SetpointManagerScheduled_FieldSet.Value;
        
        public Ironbug_SetpointManagerScheduled_Adv()
          : base("IB_SetpointManagerScheduled_Adv", "SPM_Scheduled_Adv",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerScheduled_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduled", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerScheduled();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointScheduled_adv;

        public override Guid ComponentGuid => new Guid("{AA4743D1-BDEE-4F00-94E1-E3A5B15A7F1C}");
    }
}