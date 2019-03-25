using Grasshopper.Kernel;
using System;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduledDualSetpoint : Ironbug_Component
    {
        public Ironbug_SetpointManagerScheduledDualSetpoint()
          : base("Ironbug_SetpointManagerScheduledDualSetpoint", "SPM_Dual",
              EPDoc.SetpointManagerScheduledDualSetpoint.Note,
              "Ironbug", "05:SetpointManager")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("HighTemperature", "_hiT", "High SetpointTemperature", GH_ParamAccess.item, 21.1);
            pManager.AddNumberParameter("LowTemperature", "_lowT", "Low SetpointTemperature", GH_ParamAccess.item, 12.7778);
        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduledDualSetpoint", "SPM", "TODO:...", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double hiT = 21.1;
            double lowT = 12.7778;

            DA.GetData(0, ref hiT);
            DA.GetData(1, ref lowT);


            var obj = new HVAC.IB_SetpointManagerScheduledDualSetpoint();
            obj.SetHighTemperature(hiT);
            obj.SetLowTemperature(lowT);

            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointDualScheduled;

        public override Guid ComponentGuid => new Guid("56015792-7A2B-4C40-9421-027032B71BAA");
    }
}