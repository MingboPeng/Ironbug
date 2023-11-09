using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerFollowGroundTemperature : Ironbug_DuplicableHVACComponent
    {
        private static HVAC.IB_SetpointManagerFollowGroundTemperature_FieldSet _fieldSet = HVAC.IB_SetpointManagerFollowGroundTemperature_FieldSet.Value;
        
        public Ironbug_SetpointManagerFollowGroundTemperature()
          : base("IB_SetpointManagerFollowGroundTemperature", "SPM_GrndTemp",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerFollowGroundTemperature_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary ;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerFollowGroundTemperature", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerFollowGroundTemperature();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SPM_GroundTemp;

        public override Guid ComponentGuid => new Guid("{77E1DB11-78C7-425F-93C7-1415AB383C61}");
    }
}