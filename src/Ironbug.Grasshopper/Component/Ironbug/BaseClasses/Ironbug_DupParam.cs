using Grasshopper.Kernel;
using System;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_DupParam : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Duplicate;

        public override Guid ComponentGuid => new Guid("{7EB90263-0302-432E-9592-DB492C5B9A94}");
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        public Ironbug_DupParam()
          : base("Ironbug_DupParam", "DupParam",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DupcateCounts", "_n", "Amount to be duplicated", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("dupParam", "P", "To any HVAC object's params input", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int n = 1;
            if (DA.GetData(0, ref n))
            {
                var dupobj = new DupParam();
                dupobj.Amount = n;
                DA.SetData(0, dupobj);
            }
            
            
        }
        
        
    }

    public class DupParam
    {
        private int _amount;

        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

    }
}