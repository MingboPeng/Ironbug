using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduled : Ironbug_Component
    {
        private static HVAC.IB_SetpointManagerScheduled_FieldSet _fieldSet = HVAC.IB_SetpointManagerScheduled_FieldSet.Value;
        
        /// Initializes a new instance of the Ironbug_SetpointManagerWarmest class.
        
        public Ironbug_SetpointManagerScheduled()
          : base("Ironbug_SetpointManagerScheduled", "SPM_Scheduled",
              _fieldSet.OwnerEpNote,
              "Ironbug", "05:SetpointManager")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddNumberParameter("Temperature", "_Temp", "SetpointTemperature", GH_ParamAccess.item);
            pManager.AddTextParameter("ControlVariable", "var_", _fieldSet.ControlVariable.Description, GH_ParamAccess.item);
            pManager[1].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduled", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            double temperature = 12.7778;
            DA.GetData(0, ref temperature);
            string variable = "Temperature";
            DA.GetData(1, ref variable);

            var obj = new HVAC.IB_SetpointManagerScheduled(temperature);
            obj.SetFieldValue(_fieldSet.ControlVariable, variable);
            
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointScheduled;

        public override Guid ComponentGuid => new Guid("A2FE343D-A2BA-42C3-B54E-2CBEFDE7DDA1");
    }
}