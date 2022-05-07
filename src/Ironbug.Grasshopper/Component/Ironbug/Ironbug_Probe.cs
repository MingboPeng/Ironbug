using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Probe : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_Probe()
          : base("IB_Probe", "Probe",
              "Use this component to measure variables like temperature, flow rate, etc, in the loop.\nPlace this between loopObjects.",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_Probe_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Probe", "Probe", "TODO....", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_Probe();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.probe;

        public override Guid ComponentGuid => new Guid("1590EA51-3858-4974-AE8F-AAA31411F7CE");

    }

   
}