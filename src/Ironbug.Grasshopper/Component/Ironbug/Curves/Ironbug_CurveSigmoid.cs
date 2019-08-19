using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveSigmoid : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CurveSigmoid()
          : base("Ironbug_CurveSigmoid", "CurveSigmoid",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveSigmoid_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a sigmoid curve from C1 to C5.", GH_ParamAccess.list);

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveSigmoid", "Curve", "CurveSigmoid", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveSigmoid();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 5)
                {
                    throw new Exception("5 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveSigmoid_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1C1, coeffs[0]);
                fDic.Add(fSet.Coefficient2C2, coeffs[1]);
                fDic.Add(fSet.Coefficient3C3, coeffs[2]);
                fDic.Add(fSet.Coefficient4C4, coeffs[3]);
                fDic.Add(fSet.Coefficient5C5, coeffs[4]);

                obj.SetFieldValues(fDic);
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_s;

        public override Guid ComponentGuid => new Guid("8408DC03-B1B5-4C95-83E2-B9A41112597B");
    }
}