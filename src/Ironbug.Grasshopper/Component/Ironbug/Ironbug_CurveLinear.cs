using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveLinear : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_CurveLinear()
          : base("Ironbug_CurveLinear", "CurveLinear",
              "Description",
              "Ironbug", "07:Curve",
              typeof(HVAC.Curves.IB_CurveLinear_DataFieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Coefficients", "_coeffs", "A list of coefficients for a linear curve from C1 to C2.", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveLinear", "CvLinear", "CurveLinear", GH_ParamAccess.item);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveLinear();
            var coeffs = new List<double>();

            if (DA.GetDataList(0, coeffs))
            {
                if (coeffs.Count != 2)
                {
                    throw new Exception("2 coefficient values is needed!");
                }
                var fSet = HVAC.Curves.IB_CurveLinear_DataFieldSet.Value;
                var fDic = new Dictionary<HVAC.BaseClass.IB_Field, object>();

                fDic.Add(fSet.Coefficient1Constant, coeffs[0]);
                fDic.Add(fSet.Coefficient2x, coeffs[1]);

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
                return Properties.Resources.curve_l;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("CBE88CC2-B074-4874-A620-DF9BAB454CB4"); }
        }
    }
}