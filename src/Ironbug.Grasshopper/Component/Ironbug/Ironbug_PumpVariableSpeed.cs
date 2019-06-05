using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PumpVariableSpeed : Ironbug_HVACComponent
    {
        public Ironbug_PumpVariableSpeed()
          : base("Ironbug_PumpVariableSpeed", "PumpVariable",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PumpVariableSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PumpVariableSpeed", "Pump", "connect to plantloop's supply side", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PumpVariableSpeed();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PumpV;//return null;

        public override Guid ComponentGuid => new Guid("56FFE744-FEFB-42B0-97F0-D79411287DA7");
    }
}