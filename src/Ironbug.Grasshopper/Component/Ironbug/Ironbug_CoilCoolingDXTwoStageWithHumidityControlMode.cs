using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXTwoStageWithHumidityControlMode : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CoilCoolingDXTwoStageWithHumidityControlMode()
          : base("IB_CoilCoolingDXTwoStageWithHumidityControlMode", "CoilClnDX2_HumdCtrl",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilPerformanceN1", "_cPn1_", "Coil performance object which specifies the DX coil performance for stage 1 operation without enhanced dehumidification (normal mode). Must be CoilPerformanceDXCooling", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("CoilPerformanceN1+2", "_cPn1+2_", "Coil performance object which specifies the DX coil performance for stage 1+2 operation (both stages active) without enhanced dehumidification (normal mode). Must be CoilPerformanceDXCooling", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("CoilPerformanceD1", "_cPd1_", "Coil performance object which specifies the DX coil performance for stage 1 operation with enhanced dehumidification active. Must be CoilPerformanceDXCooling", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("CoilPerformanceD1+2", "_cPd1+2_", "Coil performance object which specifies the DX coil performance for stage 1+2 operation (both stages active) with enhanced dehumidification active. Must be CoilPerformanceDXCooling", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Coil", "Coil", "CoilCoolingDXTwoStageWithHumidityControlMode", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXTwoStageWithHumidityControlMode();

            HVAC.IB_CoilPerformanceDXCooling n1 = null;
            HVAC.IB_CoilPerformanceDXCooling n12 = null;
            HVAC.IB_CoilPerformanceDXCooling d1 = null;
            HVAC.IB_CoilPerformanceDXCooling d12 = null;

            if (DA.GetData(0, ref n1)) obj.SetNormalModeStage1CoilPerformance(n1);
            if (DA.GetData(1, ref n12)) obj.SetNormalModeStage1Plus2CoilPerformance(n12); 
            if (DA.GetData(2, ref d1)) obj.SetDehumidificationMode1Stage1CoilPerformance(d1);
            if (DA.GetData(3, ref d12)) obj.SetDehumidificationMode1Stage1Plus2CoilPerformance(d12);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilDXwithH;

        public override Guid ComponentGuid => new Guid("9ACF588F-DAD7-4D6A-BB54-E8EC674B550A");

    }
}