using System;
using System.IO;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleFile: Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_ScheduleFile()
          : base("IB_ScheduleFile(WIP)", "ScheduleFile",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Schedules.IB_ScheduleFile_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary ;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("csvFile", "_csvFile", "CSV file for schedule", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Schedule", "sch", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var file = string.Empty;

            DA.GetData(0, ref file);
            if (!File.Exists(file))
            {
                throw new ArgumentException($"{file} does not exit!");
            }
            var obj = new HVAC.Schedules.IB_ScheduleFile(file);
            this.SetObjParamsTo(obj);

            DA.SetData(0, obj);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.ScheduleFile;

        public override Guid ComponentGuid => new Guid("{C1AFE176-EA12-4E68-917F-6BA237AA752D}");
    }
}