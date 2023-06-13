using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduled : Ironbug_DuplicableHVACComponent
    {
        private static HVAC.IB_SetpointManagerScheduled_FieldSet _fieldSet = HVAC.IB_SetpointManagerScheduled_FieldSet.Value;
        
        public Ironbug_SetpointManagerScheduled()
          : base("IB_SetpointManagerScheduled", "SPM_Scheduled",
              "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerScheduled_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddNumberParameter("Value", "_val", "Setpoint value", GH_ParamAccess.item);
            pManager.AddTextParameter("ControlVariable", "ctrlVar_", _fieldSet.ControlVariable.Description, GH_ParamAccess.item);
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

            var obj = new HVAC.IB_SetpointManagerScheduled(temperature, variable.Contains("Temperature"));
            obj.SetFieldValue(_fieldSet.ControlVariable, variable);


            var objs = this.SetObjDupParamsTo(obj);
            if (objs.Count() == 1)
            {
                DA.SetData(0, obj);
            }
            else
            {
                DA.SetDataList(0, objs);
            }
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointScheduled;

        public override Guid ComponentGuid => new Guid("A2FE343D-A2BA-42C3-B54E-2CBEFDE7DDA1");
    }
}