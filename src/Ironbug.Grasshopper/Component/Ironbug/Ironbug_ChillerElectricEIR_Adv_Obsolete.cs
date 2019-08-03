using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.Curves;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR_Adv : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ChillerElectricEIR_Adv()
          : base("Ironbug_ChillerElectricEIR_Advanced", "ChillerElec_Adv",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.hidden ;

        public override bool Obsolete => true;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Cooling Capacity Function of Temperature Curve", "CCFofT", 
                EPDoc.ChillerElectricEIR.Field_CoolingCapacityFunctionOfTemperatureCurveName, GH_ParamAccess.item);

            pManager.AddGenericParameter("Electric Input to Cooling Output Ratio Function of Temperature Curve", "EItoCORFofT", 
                EPDoc.ChillerElectricEIR.Field_ElectricInputToCoolingOutputRatioFunctionOfTemperatureCurveName, GH_ParamAccess.item);

            pManager.AddGenericParameter("Electric Input to Cooling Output Ratio Function of Part Load Ratio Curve", "EItoCORFofPLR", 
                EPDoc.ChillerElectricEIR.Field_ElectricInputToCoolingOutputRatioFunctionOfPartLoadRatioCurveName, GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ChillerElectricEIR", "Chiller", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ChillerElectricEIR_CondenserLoop", "ToCondenser", "Connect to condenser loop's demand side.", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The basic ChillerElectricEIR now supports setting up the performance curves, please use the basic version instead!\nThis component will be removed in the future version.");
            IB_CurveBiquadratic CCFofT = null;
            IB_CurveBiquadratic EItoCORFofT = null;
            IB_CurveQuadratic EItoCORFofPLR = null;

            DA.GetData(0, ref CCFofT);
            DA.GetData(1, ref EItoCORFofT);
            DA.GetData(2, ref EItoCORFofPLR);

            var obj = new HVAC.IB_ChillerElectricEIR(CCFofT, EItoCORFofT, EItoCORFofPLR);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Chiller_adv;
        
        public override Guid ComponentGuid => new Guid("EB46B382-02B6-42F4-95D9-0A58178AB047");
    }
}