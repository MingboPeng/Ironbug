using System;

using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirTerminalSingleDuctVAVNoReheat : Ironbug_HVACComponent
    {
        public Ironbug_AirTerminalSingleDuctVAVNoReheat()
          : base("Ironbug_AirTerminalSingleDuctVAVNoReheat", "VAVNoReheat",
              "Description",
              "Ironbug", "03:AirTerminals",
              typeof(IB_AirTerminalSingleDuctVAVNoReheat_FieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirTerminalSingleDuctVAVNoReheat", "AT", "connect to Zone", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_AirTerminalSingleDuctVAVNoReheat();
            
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.VAVBoxNoReheat;

        public override Guid ComponentGuid => new Guid("53E65AD0-072D-44D7-BC55-55245A6FE8F3");
    }
}