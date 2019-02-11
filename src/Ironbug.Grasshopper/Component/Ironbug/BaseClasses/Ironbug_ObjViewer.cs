using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ObjViewer : GH_Component
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ObjViewer;

        public override Guid ComponentGuid => new Guid("ACA051EE-C66B-484A-A127-8A373D6F5A57");
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public Ironbug_ObjViewer()
          : base("Ironbug_ObjViewer", "ObjViewer",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "_obj", "object to be check", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "Object data includes its children's data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_ModelObject ibObj = null;
            if (DA.GetData(0, ref ibObj))
            {
                var strs = ibObj.ToStrings();
                DA.SetDataList(0, strs);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "input object is not a valid OpenStudio object");
            }
            
            
        }
        
        
    }
}