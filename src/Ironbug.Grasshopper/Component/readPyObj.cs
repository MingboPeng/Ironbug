using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper
{
    public class readPyObj : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the readPyObj class.
        /// </summary>
        public readPyObj()
          : base("readPyObj", "Nickname",
              "Description",
              "Mingbo_Dev", "test")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("wea", "wea", "WeatherObj", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("header", "header", "epw header", GH_ParamAccess.item);
            pManager.AddGenericParameter("weaC", "weaC", "WeatherObj", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            dynamic rawObj = null;

            DA.GetData(0, ref rawObj);
            
            DA.SetData(0, new Ironbug.Ladybug.Wea(rawObj).Header);
            DA.SetData(1, rawObj);



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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8ddc0cd7-591a-43fb-8701-f7c09cdd0ff0"); }
        }
    }
}