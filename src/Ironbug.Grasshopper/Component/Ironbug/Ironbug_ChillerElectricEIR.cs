using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.HVAC.Curves;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerElectricEIR : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ChillerElectricEIR class.
        /// </summary>
        public Ironbug_ChillerElectricEIR()
          : base("Ironbug_ChillerElectricEIR_WaterCooled", "Chiller_WaterCooled",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerElectricEIR_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
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

            IB_CurveBiquadratic CCFofT = WaterCooledCurve1();
            
            IB_CurveBiquadratic EItoCORFofT = WaterCooledCurve2();
            IB_CurveQuadratic EItoCORFofPLR = WaterCooledCurve3();

            var obj = new HVAC.IB_ChillerElectricEIR(CCFofT, EItoCORFofT, EItoCORFofPLR);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }

        IB_CurveBiquadratic WaterCooledCurve1()
        {
            IB_CurveBiquadratic cv = new IB_CurveBiquadratic();

            var fSet = IB_CurveBiquadratic_FieldSet.Value;
            var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

            fDic.Add(fSet.Coefficient1Constant, 0.258);
            fDic.Add(fSet.Coefficient2x, 0.0389);
            fDic.Add(fSet.Coefficient3xPOW2, -0.000217);
            fDic.Add(fSet.Coefficient4y, 0.0469);
            fDic.Add(fSet.Coefficient5yPOW2, -0.000943);
            fDic.Add(fSet.Coefficient6xTIMESY, -0.000343);

            fDic.Add(fSet.MinimumValueofx, 5);
            fDic.Add(fSet.MaximumValueofx, 10);
            fDic.Add(fSet.MinimumValueofy, 24);
            fDic.Add(fSet.MaximumValueofy, 35);

            cv.SetFieldValues(fDic);

            return cv;
        }

        IB_CurveBiquadratic WaterCooledCurve2()
        {
            IB_CurveBiquadratic cv = new IB_CurveBiquadratic();

            var fSet = IB_CurveBiquadratic_FieldSet.Value;
            var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

            fDic.Add(fSet.Coefficient1Constant, 0.934);
            fDic.Add(fSet.Coefficient2x, -0.0582);
            fDic.Add(fSet.Coefficient3xPOW2, 0.0045);
            fDic.Add(fSet.Coefficient4y, 0.00243);
            fDic.Add(fSet.Coefficient5yPOW2, 0.000486);
            fDic.Add(fSet.Coefficient6xTIMESY, -0.00122);

            fDic.Add(fSet.MinimumValueofx, 5);
            fDic.Add(fSet.MaximumValueofx, 10);
            fDic.Add(fSet.MinimumValueofy, 24);
            fDic.Add(fSet.MaximumValueofy, 35);

            cv.SetFieldValues(fDic);

            return cv;
        }

        IB_CurveQuadratic WaterCooledCurve3()
        {
            IB_CurveQuadratic cv = new IB_CurveQuadratic();

            var fSet = IB_CurveQuadratic_FieldSet.Value;
            var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

            fDic.Add(fSet.Coefficient1Constant, 0.222903);
            fDic.Add(fSet.Coefficient2x, 0.313387);
            fDic.Add(fSet.Coefficient3xPOW2, 0.46371);
            
            cv.SetFieldValues(fDic);

            return cv;
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Chiller;
        
        public override Guid ComponentGuid => new Guid("9cba9235-6cc3-498e-b653-6eda9870d276");
    }
}