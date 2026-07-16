using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveFunctionalPressureDrop : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveFunctionalPressureDrop()
          : base("IB_CurveFunctionalPressureDrop", "CurveFunctionalPressureDrop",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveFunctionalPressureDrop_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveFunctionalPressureDrop", "Curve", "CurveFunctionalPressureDrop", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveFunctionalPressureDrop();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("4BB8D9A4-C5CF-403B-AFB9-6DD86183D25E");
    }
}