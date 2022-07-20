using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXTwoStageWithHumidityControlMode : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        
        public Ironbug_CoilCoolingDXTwoStageWithHumidityControlMode()
          : base("IB_CoilCoolingDXTwoStageWithHumidityControlMode", "CoilClnDX3",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXTwoStageWithHumidityControlMode", "Coil", "CoilCoolingDXTwoStageWithHumidityControlMode", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXTwoStageWithHumidityControlMode();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        //protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCDX2;

        public override Guid ComponentGuid => new Guid("9ACF588F-DAD7-4D6A-BB54-E8EC674B550A");

    }
}