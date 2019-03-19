using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveBicubic : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_CurveBicubic()
          : base("Ironbug_CurveBicubic", "CurveBicubic",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveBicubic_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a bicubic curve from C1 to C10.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveBicubic", "Curve", "CurveBicubic", GH_ParamAccess.item);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveBicubic();
            var coeffs = new List<double>();

            if (DA.GetDataList(0,coeffs))
            {
                if (coeffs.Count!=10)
                {
                    throw new Exception("10 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveBicubic_DataFieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);
                fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
                fDic.Add(fSet.Coefficient4y, coeffs[3]);
                fDic.Add(fSet.Coefficient5yPOW2, coeffs[4]);
                fDic.Add(fSet.Coefficient6xTIMESY, coeffs[5]);
                fDic.Add(fSet.Coefficient7xPOW3, coeffs[6]);
                fDic.Add(fSet.Coefficient8yPOW3, coeffs[7]);
                fDic.Add(fSet.Coefficient9xPOW2TIMESY, coeffs[8]);
                fDic.Add(fSet.Coefficient10xTIMESYPOW2, coeffs[9]);

                obj.SetFieldValues(fDic);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.curve_bc;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1D429D0B-AC3A-408B-8074-87EDCB655981"); }
        }
    }
}