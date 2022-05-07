using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDXMultiSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoilHeatingDXMultiSpeed()
          : base("IB_CoilHeatingDXMultiSpeed", "CoilHtn_DXM",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingDXMultiSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXMultiSpeed", "Coil", "CoilHeatingDXMultiSpeed", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingDXMultiSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "This obj is not fully finished by OpenStudio, stay tuned!");
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHDXM;

        public override Guid ComponentGuid => new Guid("09283357-3680-4569-9149-980153F6A0E6");
    }
}