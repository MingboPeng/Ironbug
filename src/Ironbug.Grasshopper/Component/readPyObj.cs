using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper
{
    public class readPyObj : GH_Component
    {
        
        /// Initializes a new instance of the readPyObj class.
        
        public readPyObj()
          : base("readPyObj", "Nickname",
              "Description",
              "Mingbo_Dev", "test")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("wea", "wea", "WeatherObj", GH_ParamAccess.item);
        }

        
        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("header", "header", "epw header", GH_ParamAccess.item);
            pManager.AddGenericParameter("weaC", "weaC", "WeatherObj", GH_ParamAccess.item);
        }

        
        
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            dynamic rawObj = null;

            DA.GetData(0, ref rawObj);
            
            DA.SetData(0, new Ironbug.Ladybug.Wea(rawObj).Header);
            DA.SetData(1, rawObj);



        }

        

        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        

        
        public override Guid ComponentGuid
        {
            get { return new Guid("8ddc0cd7-591a-43fb-8701-f7c09cdd0ff0"); }
        }
    }
}