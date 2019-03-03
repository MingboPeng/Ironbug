using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Duct : Ironbug_HVACComponent
    {
        
        /// <summary>
        /// Initializes a new instance of the Ironbug_FanConstantVolume class.
        /// </summary>
        public Ironbug_Duct()
          : base("Ironbug_Duct", "Duct",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_Duct_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Duct", "Duct", "Duct", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_Duct();

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
                return Resources.duct;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7CC476E8-920B-4115-9938-30329AE5F55A"); }
        }

        
    }

   
}