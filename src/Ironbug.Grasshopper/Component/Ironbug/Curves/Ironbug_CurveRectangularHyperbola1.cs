using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveRectangularHyperbola1 : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveRectangularHyperbola1()
          : base("IB_CurveRectangularHyperbola1", "CurveRectangularHyperbola1",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveRectangularHyperbola1_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveRectangularHyperbola1", "Curve", "CurveRectangularHyperbola1", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveRectangularHyperbola1();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("FF588C30-08B0-4C1A-9664-43BACBD845AD");
    }
}