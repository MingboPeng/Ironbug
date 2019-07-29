using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleRule_Obsolete : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_ScheduleRule_Obsolete()
          : base("Ironbug_ScheduleRule", "ScheduleRule",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Schedules.IB_ScheduleRule_FieldSet))
        {
        }

        public override bool Obsolete => true;
        public override GH_Exposure Exposure => GH_Exposure.hidden;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "value", "One value for all day or 24 value for each hour", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ScheduleRule", "schRule", "To Ironbug_Schedule", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var values = new List<double>();
            DA.GetDataList(0, values);
            if (values.Count ==1)
            {
                var day = new HVAC.Schedules.IB_ScheduleDay(values[0]);
                var schRule = new HVAC.Schedules.IB_ScheduleRule(day);

                this.SetObjParamsTo(schRule);
                DA.SetData(0, schRule);
            }
            else
            {
                if (values.Count != 24) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Need 24 valves");
                var day = new HVAC.Schedules.IB_ScheduleDay(values);
                var schRule = new HVAC.Schedules.IB_ScheduleRule(day);
                
                this.SetObjParamsTo(schRule);
                DA.SetData(0, schRule);
            }

           
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule_Rule;

        public override Guid ComponentGuid => new Guid("5C9F9494-93DF-4F5A-9718-319866F61C05");
    }
}