using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerAbsorptionIndirect : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_ChillerAbsorptionIndirect()
          : base("IB_ChillerAbsorptionIndirect", "ChillerAbsorptionInd",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_ChillerAbsorptionIndirect_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller supply", "CHW_supply", "connect to chilled water loop's supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("Condenser demand", "CW_demand", "connect to condenser water loop's supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("Generator demand", "GeneratorLoop", "(Optional) connect to a steam or hot water loop's demand side.", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ChillerAbsorptionIndirect();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
            DA.SetDataList(2, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("2B3AF341-6E43-4061-AA9B-B2A7BD1C0423");
    }
}