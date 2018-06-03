using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerWarmest : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SetpointManagerWarmest class.
        /// </summary>
        public Ironbug_SetpointManagerWarmest()
          : base("Ironbug_SetpointManagerWarmest", "SpWarmest",
              "Description",
              "Ironbug", "05:SetpointManager")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("minTemperature", "_minT", "MinimumSetpointTemperature", GH_ParamAccess.item);
            pManager.AddNumberParameter("maxTemperature", "_maxT", "MaximumSetpointTemperature", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerWarmest", "SpWarmest", "TODO:...", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerWarmest();
            double minT = 0;
            double maxT = 0;
            DA.GetData(0, ref minT);
            DA.GetData(1, ref maxT);

            
            var fieldSet = HVAC.IB_SetpointManagerWarmest_DataFieldSet.Value;

            obj.SetFieldValue(fieldSet.MinimumSetpointTemperature, minT);
            obj.SetFieldValue(fieldSet.MaximumSetpointTemperature, maxT);
            
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
                return Properties.Resources.SetPointWarmest;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9d1e18c2-392e-47ea-8f29-e5407bbd3278"); }
        }
    }
}