using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveQuintLinear : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CurveQuintLinear()
          : base("IB_CurveQuintLinear", "CurveQuintLinear",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveQuintLinear_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveQuintLinear", "Curve", "CurveQuintLinear", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveQuintLinear();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("D90EFE4D-F388-4017-BBCC-C82804BA6975");
    }
}