using System;
using Grasshopper.Kernel;
using Ironbug.HVAC.Curves;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR_Adv : Ironbug_HVACComponent
    {
        public Ironbug_ChillerElectricEIR_Adv()
          : base("Ironbug_ChillerElectricEIR_Advanced", "ChillerElec_Adv",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Cooling Capacity Function of Temperature Curve", "CCFofT", 
                EPDoc.ChillerElectricEIR.Field_CoolingCapacityFunctionOfTemperatureCurveName, GH_ParamAccess.item);

            pManager.AddGenericParameter("Electric Input to Cooling Output Ratio Function of Temperature Curve", "EItoCORFofT", 
                EPDoc.ChillerElectricEIR.Field_ElectricInputToCoolingOutputRatioFunctionOfTemperatureCurveName, GH_ParamAccess.item);

            pManager.AddGenericParameter("Electric Input to Cooling Output Ratio Function of Part Load Ratio Curve", "EItoCORFofPLR", 
                EPDoc.ChillerElectricEIR.Field_ElectricInputToCoolingOutputRatioFunctionOfPartLoadRatioCurveName, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ChillerElectricEIR", "Chiller", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ChillerElectricEIR_CondenserLoop", "ToCondenser", "Connect to condenser loop's demand side.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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