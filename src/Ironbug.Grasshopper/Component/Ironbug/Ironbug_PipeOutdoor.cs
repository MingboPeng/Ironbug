using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PipeOutdoor : Ironbug_DuplicatableHVACComponent
    {
        public Ironbug_PipeOutdoor()
          : base("Ironbug_PipeOutdoor", "PipeOutdoor",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PipeOutdoor_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PipeOutdoor", "Pipe", "PipeOutdoor", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PipeOutdoor();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.PipeOutdoor;

        public override Guid ComponentGuid => new Guid("2E05B317-AC4A-458E-9089-A6BDB88FF245");


    }

   
}