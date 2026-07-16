using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveQuadraticLinear : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveQuadraticLinear()
          : base("IB_CurveQuadraticLinear", "CurveQuadraticLinear",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveQuadraticLinear_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveQuadraticLinear", "Curve", "CurveQuadraticLinear", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveQuadraticLinear();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("CB0D31CA-AA9B-4113-B654-94CC876B35C2");
    }
}