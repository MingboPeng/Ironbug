using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatPumpPlantLoopEIRHeating_Air : Ironbug_HVACWithParamComponent
    {
        public Ironbug_HeatPumpPlantLoopEIRHeating_Air()
          : base("IB_HeatPumpPlantLoopEIRHeating_AirSource", "HtnHP_AirSource",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatPumpPlantLoopEIRHeating_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CompanionCoolingHeatPump", "CompanionClnHP", "CompanionCoolingHeatPump", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatPumpPlantLoopEIRHeating", "HtnHP", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_HeatPumpPlantLoopEIRCooling hp = null;
            var obj = new HVAC.IB_HeatPumpPlantLoopEIRHeating();
            if (DA.GetData(0, ref hp) && hp != null)
            {
                obj.SetCompanionCoolingHeatPump(hp);
            }

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatPumpW2W_Heating;

        public override Guid ComponentGuid => new Guid("35363E35-0D83-4439-A9D2-F7B00543DE8C");
    }
}