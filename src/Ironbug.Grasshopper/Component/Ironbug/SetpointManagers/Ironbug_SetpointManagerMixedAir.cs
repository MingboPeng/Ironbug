using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerMixedAir : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_SetpointManagerMixedAir()
          : base("IB_SetpointManagerMixedAir", "SPM_MxAir",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerMixedAir_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary ;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerMixedAir", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerMixedAir();

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetpointMxAir;

        public override Guid ComponentGuid => new Guid("{721290AC-8BA7-473B-8C83-39BD5E6B579F}");
    }
}