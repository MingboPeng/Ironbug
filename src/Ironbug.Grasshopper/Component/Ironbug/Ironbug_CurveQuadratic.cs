using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveQuadratic : Ironbug_HVACComponent
    {
        
        public Ironbug_CurveQuadratic()
          : base("Ironbug_CurveQuadratic", "CurveQuadratic",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveQuadratic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a quadratic curve from C1 to C3.", GH_ParamAccess.list);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveQuadratic", "Curve", "CurveQuadratic", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveQuadratic();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 3)
                {
                    throw new Exception("3 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveQuadratic_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);
                fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);

                obj.SetFieldValues(fDic);
            }
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }




        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_q;

        public override Guid ComponentGuid => new Guid("DB7C037E-D1AC-488D-9156-DF60B2FC2C69");
    }
}