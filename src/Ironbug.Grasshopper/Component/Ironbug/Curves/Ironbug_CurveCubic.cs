using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveCubic : Ironbug_HVACComponent
    {
        
        public Ironbug_CurveCubic()
          : base("Ironbug_CurveCubic", "CurveCubic",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveCubic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a cubic curve from C1 to C4.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveCubic", "Curve", "CurveCubic", GH_ParamAccess.item);
        }
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveCubic();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 4)
                {
                    throw new Exception("4 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveCubic_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);
                fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
                fDic.Add(fSet.Coefficient4xPOW3, coeffs[3]);

                obj.SetFieldValues(fDic);
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_c;

        public override Guid ComponentGuid => new Guid("AD467DE4-EEFD-484E-8ED2-E0D30D49244F");
    }
}