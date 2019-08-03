using Grasshopper.Kernel;
using System;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerOutdoorAirPretreat : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_SetpointManagerOutdoorAirPretreat()
          : base("Ironbug_SetpointManagerOutdoorAirPretreat", "SPM_OAPretreat",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerOutdoorAirPretreat_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerOutdoorAirPretreat", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerOutdoorAirPretreat();
            
            this.SetObjParamsTo(obj);

            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointOARetreat;

        public override Guid ComponentGuid => new Guid("D2A9417D-0A5D-4F08-8EBC-5F0F05B36495");
    }
}