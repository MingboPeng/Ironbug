using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXTwoSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        
        public Ironbug_CoilCoolingDXTwoSpeed()
          : base("Ironbug_CoilCoolingDXTwoSpeed", "CoilClnDX2",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXTwoSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXTwoSpeed", "Coil", "CoilCoolingDXTwoSpeed", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXTwoSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCDX2;

        public override Guid ComponentGuid => new Guid("5237B52C-06E2-485B-B414-9A3455DA2A72");
    }
}