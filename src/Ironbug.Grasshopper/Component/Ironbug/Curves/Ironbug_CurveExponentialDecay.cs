using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveExponentialDecay : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveExponentialDecay()
          : base("IB_CurveExponentialDecay", "CurveExponentialDecay",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveExponentialDecay_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveExponentialDecay", "Curve", "CurveExponentialDecay", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveExponentialDecay();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("D2ADE549-ECFD-40EF-A60A-0C6C997C6F5D");
    }
}