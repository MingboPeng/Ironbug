using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveExponent : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveExponent()
          : base("IB_CurveExponent", "CurveExponent",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveLinear_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for an exponent curve from C1 to C3.", GH_ParamAccess.list);

        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveExponent", "Curve", "CurveExponent", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveExponent();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 3)
                {
                    throw new Exception("3 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveExponent_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2Constant, coeffs[1]);
                fDic.Add(fSet.Coefficient3Constant, coeffs[2]);

                obj.SetFieldValues(fDic);
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_e;

        public override Guid ComponentGuid => new Guid("707A05C7-CA9B-40C3-989C-20EEE7289457");
    }
}