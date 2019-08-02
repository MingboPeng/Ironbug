using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingElectric : Ironbug_LoopObjectComponent
    {
        public Ironbug_CoilHeatingElectric()
          : base("Ironbug_CoilHeatingElectric", "CoilHtnElec",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingElectric_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingElectric", "Coil", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingElectric();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHE;
        
        public override Guid ComponentGuid => new Guid("dc8ed4d8-4435-4134-b1e8-06362bb6d411");
    }
}