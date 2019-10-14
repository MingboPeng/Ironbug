using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerScheduled_Adv : Ironbug_DuplicableHVACComponent
    {
        private static HVAC.IB_SetpointManagerScheduled_FieldSet _fieldSet = HVAC.IB_SetpointManagerScheduled_FieldSet.Value;
        
        public Ironbug_SetpointManagerScheduled_Adv()
          : base("Ironbug_SetpointManagerScheduled_Adv", "SPM_Scheduled_Adv",
              "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerScheduled_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddGenericParameter("Schedule", "_Sch", "A schedule for setpoint", GH_ParamAccess.item);
            pManager.AddTextParameter("ControlVariable", "var_", _fieldSet.ControlVariable.Description, GH_ParamAccess.item);
            pManager[1].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerScheduled", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            HVAC.Schedules.IB_ScheduleRuleset sch = null;
            DA.GetData(0, ref sch);
            string variable = "Temperature";
            DA.GetData(1, ref variable);

            try
            {
                var obj = new HVAC.IB_SetpointManagerScheduled(sch);
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
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unable to set Object of type 'OS:Schedule:Ruleset'"))
                {
                    throw new ArgumentException("It seems the type of schedule does not match this setpoint manager control variable, please double check if it is 'Temperature', 'Fraction', or 'Dimensionless', etc. \n\nHere are error message:" + ex.Message);

                }
                else
                {
                    throw ex;
                }
               
            }
            


        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointScheduled_adv;

        public override Guid ComponentGuid => new Guid("{AA4743D1-BDEE-4F00-94E1-E3A5B15A7F1C}");
    }
}