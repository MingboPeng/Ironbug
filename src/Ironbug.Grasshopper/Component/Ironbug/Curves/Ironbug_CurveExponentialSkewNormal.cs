using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveExponentialSkewNormal : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveExponentialSkewNormal()
          : base("IB_CurveExponentialSkewNormal", "CurveExponentialSkewNormal",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveExponentialSkewNormal_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveExponentialSkewNormal", "Curve", "CurveExponentialSkewNormal", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveExponentialSkewNormal();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("2B00804C-D955-4465-9B42-ADD07080A60F");
    }
}