using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveRectangularHyperbola2 : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveRectangularHyperbola2()
          : base("IB_CurveRectangularHyperbola2", "CurveRectangularHyperbola2",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveRectangularHyperbola2_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveRectangularHyperbola2", "Curve", "CurveRectangularHyperbola2", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveRectangularHyperbola2();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("C8E677EA-9C14-4357-9013-B58EF63A3B94");
    }
}