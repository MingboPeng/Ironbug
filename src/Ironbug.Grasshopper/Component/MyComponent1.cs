using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class MyComponent1 : GH_Component
    {
        
        /// Initializes a new instance of the MyComponent1 class.
        
        public MyComponent1()
          : base("MyComponent1", "Nickname",
              "Description",
              "Mingbo_Dev", "test")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
        
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("epw", "epw", "epw Weather file", GH_ParamAccess.item);
        }

        
        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("wea", "wea", "Weather file", GH_ParamAccess.item);
        }

        
        
        
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string epwFile = string.Empty;

            DA.GetData(0, ref epwFile);

            var aa = new Ladybug.Wea();
            var epw  = aa.From_EpwFile(epwFile);
            //var header = epw.Header;

            DA.SetData(0, aa);

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
            get { return new Guid("37675798-e37a-4c64-b2dc-766bd1f2589f"); }
        }
    }
}