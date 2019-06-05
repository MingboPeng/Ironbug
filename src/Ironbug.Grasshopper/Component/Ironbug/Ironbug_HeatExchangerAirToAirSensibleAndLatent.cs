using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerAirToAirSensibleAndLatent : Ironbug_HVACComponent
    {
        public Ironbug_HeatExchangerAirToAirSensibleAndLatent()
          : base("Ironbug_HeatExchangerAirToAirSensibleAndLatent", "HeatExchanger_Air",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerAirToAirSensibleAndLatent_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatExchangerAirToAirSensibleAndLatent", "HX", "to OutdoorAirSystem", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatExchangerAirToAirSensibleAndLatent();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatEx_Air;

        public override Guid ComponentGuid => new Guid("7E61D04D-038A-4ED3-BA95-CC2AA6DC5AC2");
    }
}