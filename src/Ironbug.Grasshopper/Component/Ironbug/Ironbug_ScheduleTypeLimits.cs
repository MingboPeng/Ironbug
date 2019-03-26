using System;
using System.Linq;
using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleTypeLimits : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_SizingZone class.
        /// </summary>
        public Ironbug_ScheduleTypeLimits()
          : base("Ironbug_ScheduleTypeLimits", "ScheduleType",
              HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value.OwnerEpNote,
              "Ironbug", "07:Curve & Load")
        {
        }
        
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("LowerLimitValue", "LowLimit", HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value.LowerLimitValue.Description, GH_ParamAccess.item, 0);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("UpperLimitValue", "UpLimit", HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value.UpperLimitValue.Description, GH_ParamAccess.item, 1.0);
            pManager[1].Optional = true;
            pManager.AddTextParameter("UnitType", "UnitType", HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value.UnitType.Description, GH_ParamAccess.item, "Dimensionless");
            pManager[2].Optional = true;
            pManager.AddTextParameter("NumericType", "Numeric", HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value.NumericType.Description, GH_ParamAccess.item, "Discrete");
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ScheduleTypeLimits", "schType", "To Ironbug_Schedule", GH_ParamAccess.item);
        }
        
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var attributes = HVAC.Schedules.IB_ScheduleTypeLimits_FieldSet.Value;
            var objAttris = new System.Collections.Generic.Dictionary<IB_Field, object>() { };
            double lowLimit = 0.0;
            double upLimit = 1.0;
            string type = string.Empty;
            string numType = string.Empty;
            if (DA.GetData(0, ref lowLimit)) objAttris.Add(attributes.LowerLimitValue, lowLimit);
            if (DA.GetData(1, ref upLimit)) objAttris.Add(attributes.UpperLimitValue, upLimit);
            if (DA.GetData(2, ref type)) {
                if (attributes.UnitType.ValidData.Any())
                {
                    if (attributes.UnitType.ValidData.Contains(type))
                    {
                        objAttris.Add(attributes.UnitType, type);
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Double check the typo in UnitType!");
                    }
                }
            }
            if (DA.GetData(3, ref numType)) {
                if (attributes.NumericType.ValidData.Any())
                {
                    if (attributes.NumericType.ValidData.Contains(numType))
                    {
                        objAttris.Add(attributes.NumericType, numType);
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Double check the typo in NumericType!");
                    }
                }
            } 

            var obj = new HVAC.Schedules.IB_ScheduleTypeLimits();
            obj.SetFieldValues(objAttris);
            DA.SetData(0, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule_Type;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("C090F57C-EC4F-417A-9156-BA1BC4047E75");
    }
}