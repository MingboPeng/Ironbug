using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveBiquadratic : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveBiquadratic()
          : base("IB_CurveBiquadratic", "CurveBiquadratic",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveBiquadratic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a biquadratic curve from C1 to C6.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveBiquadratic", "Curve", "CurveBiquadratic", GH_ParamAccess.item);
        }
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveBiquadratic();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 6)
                {
                    throw new Exception("6 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveBiquadratic_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);
                fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
                fDic.Add(fSet.Coefficient4y, coeffs[3]);
                fDic.Add(fSet.Coefficient5yPOW2, coeffs[4]);
                fDic.Add(fSet.Coefficient6xTIMESY, coeffs[5]);

                obj.SetFieldValues(fDic);
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_bq;

        public override Guid ComponentGuid => new Guid("D9DEAEF5-F36E-4724-A489-58E4C11869DA");
    }
}