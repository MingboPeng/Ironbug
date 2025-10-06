using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ElectricLoadCenterTransformer : Ironbug_HVACWithParamComponent
    {
        public Ironbug_ElectricLoadCenterTransformer()
          : base("IB_ElectricLoadCenterTransformer", "Transformer",
              "Description",
              "Ironbug", "08:ElectricLoadCenter",
              typeof(HVAC.IB_ElectricLoadCenterTransformer_FieldSet))
        {
            
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("transformer", "transformer", "transformer", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ElectricLoadCenterTransformer();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.Transformer;

        public override Guid ComponentGuid => new Guid("826B1B36-D27A-4096-ACB1-9D9DC56D199C");


    }

   
}