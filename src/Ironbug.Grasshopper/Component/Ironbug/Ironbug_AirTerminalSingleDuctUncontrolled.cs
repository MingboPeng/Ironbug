using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctUncontrolled : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctVAVReheat class.
        
        public Ironbug_AirTerminalSingleDuctUncontrolled()
          : base("Ironbug_AirTerminalSingleDuctUncontrolled", "Diffuser",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctUncontrolled_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctUncontrolled", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctUncontrolled();
            
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);

        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.AirTerminalUncontrolled;

        public override Guid ComponentGuid => new Guid("623EC8EE-FE37-44B7-BBC7-2BA62C597BC4");
    }
}