using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveFanPressureRise : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_CurveFanPressureRise()
          : base("Ironbug_CurveFanPressureRise", "CurveFanPressure",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveCubic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a FanPressureRise curve from C1 to C4.", GH_ParamAccess.list);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveFanPressureRise", "Curve", "CurveFanPressureRise", GH_ParamAccess.item);
        }
        
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveFanPressureRise();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 4)
                {
                    throw new Exception("4 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveFanPressureRise_FieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1C1, coeffs[0]);
                fDic.Add(fSet.Coefficient2C2, coeffs[1]);
                fDic.Add(fSet.Coefficient3C3, coeffs[2]);
                fDic.Add(fSet.Coefficient4C4, coeffs[3]);

                obj.SetFieldValues(fDic);
            }
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_fan;

        public override Guid ComponentGuid => new Guid("D08F916E-E8D7-461C-AB2B-5863BA58A724");
    }
}