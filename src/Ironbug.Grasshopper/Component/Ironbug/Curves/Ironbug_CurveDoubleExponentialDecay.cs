using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveDoubleExponentialDecay : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveDoubleExponentialDecay()
          : base("IB_CurveDoubleExponentialDecay", "CurveDoubleExponentialDecay",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveDoubleExponentialDecay_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveDoubleExponentialDecay", "Curve", "CurveDoubleExponentialDecay", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveDoubleExponentialDecay();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("9CB6F10A-C7E2-459D-BEAD-1E01B2D4EED3");
    }
}