using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveQuartic : Ironbug_HVACComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_CurveQuartic()
          : base("Ironbug_CurveQuartic", "CvQuartic",
              "Description",
              "Ironbug", "07:Curve",
              typeof(HVAC.Curves.IB_CurveQuartic_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a quartic curve from C1 to C5.", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveQuartic", "CvQuartic", "CurveQuartic", GH_ParamAccess.item);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveQuartic();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 5)
                {
                    throw new Exception("5 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveQuartic_DataFieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);
                fDic.Add(fSet.Coefficient3xPOW2, coeffs[2]);
                fDic.Add(fSet.Coefficient4xPOW3, coeffs[3]);
                fDic.Add(fSet.Coefficient5xPOW4, coeffs[4]);

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
                return Properties.Resources.curve_qt;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9082B8C1-102A-4C5A-A835-857118E78A88"); }
        }
    }
}