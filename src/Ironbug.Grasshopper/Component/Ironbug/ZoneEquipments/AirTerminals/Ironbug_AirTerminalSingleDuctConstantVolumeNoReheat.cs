using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctUncontrolled : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_AirTerminalSingleDuctVAVReheat class.
        
        public Ironbug_AirTerminalSingleDuctUncontrolled()
          : base("Ironbug_AirTerminalSingleDuctConstantVolumeNoReheat", "Diffuser",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctConstantVolumeNoReheat_FieldSet))
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctConstantVolumeNoReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctConstantVolumeNoReheat();
            
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.AirTerminalUncontrolled;

        public override Guid ComponentGuid => new Guid("623EC8EE-FE37-44B7-BBC7-2BA62C597BC4");
    }
}