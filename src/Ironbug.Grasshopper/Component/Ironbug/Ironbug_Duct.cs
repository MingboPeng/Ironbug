using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Duct : Ironbug_HVACComponent
    {
        public Ironbug_Duct()
          : base("Ironbug_Duct", "Duct",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_Duct_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Duct", "Duct", "Duct", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_Duct();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Resources.duct;

        public override Guid ComponentGuid => new Guid("7CC476E8-920B-4115-9938-30329AE5F55A");


    }

   
}