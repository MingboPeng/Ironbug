using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerDesiccantBalancedFlow : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_HeatExchangerDesiccantBalancedFlow()
          : base("IB_HeatExchangerDesiccantBalancedFlow", "HeatExchanger_Desiccant",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerDesiccantBalancedFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatExchangerDesiccantBalancedFlow", "HX", "to OutdoorAirSystem", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatExchangerDesiccantBalancedFlow();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("CDD4E238-9929-4608-953F-ED5838F1BB51");
    }
}