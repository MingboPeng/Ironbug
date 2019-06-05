using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CurveTriquadratic : Ironbug_HVACComponent
    {
        public Ironbug_CurveTriquadratic()
          : base("Ironbug_CurveTriquadratic", "CurveTriquadratic",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Curves.IB_CurveTriquadratic_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CurveTriquadratic", "Curve", "CurveTriquadratic", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.Curves.IB_CurveTriquadratic();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.curve_tq;

        public override Guid ComponentGuid => new Guid("A79FB106-F26E-4657-A91B-1C59F1955FA6");
    }
}