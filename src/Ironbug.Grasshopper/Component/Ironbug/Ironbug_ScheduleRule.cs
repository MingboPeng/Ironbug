using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleRule : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_ScheduleRule()
          : base("Ironbug_ScheduleRule", "ScheduleRule",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Schedules.IB_ScheduleRule_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "value", "One value for all day or 24 value for each hour", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ScheduleRule", "schRule", "To Ironbug_Schedule", GH_ParamAccess.item);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var values = new List<double>();
            DA.GetDataList(0, values);
            if (values.Count ==1)
            {
                DA.SetData(0, new HVAC.Schedules.IB_ScheduleRuleset(values[0]));
                return;
            }

            if (values.Count != 24) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Need 24 valves");
            var day = new HVAC.Schedules.IB_ScheduleDay(values);
            var schRule = new HVAC.Schedules.IB_ScheduleRule(day);


            this.SetObjParamsTo(schRule);
            DA.SetData(0, schRule);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule_Rule;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("5C9F9494-93DF-4F5A-9718-319866F61C05");
    }
}