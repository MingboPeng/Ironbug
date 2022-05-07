using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDXSingleSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoilHeatingDXSingleSpeed()
          : base("IB_CoilHeatingDXSingleSpeed", "CoilHtn_DX1",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingDXSingleSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXSingleSpeed", "Coil", "CoilHeatingDXSingleSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingDXSingleSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHDX1;
        
        public override Guid ComponentGuid => new Guid("9C0338B1-C2FE-4AEC-8219-BCF87128BF5D");
    }
}