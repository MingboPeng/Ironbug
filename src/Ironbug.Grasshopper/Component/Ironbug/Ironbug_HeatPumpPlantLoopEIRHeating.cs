using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatPumpPlantLoopEIRHeating : Ironbug_HVACWithParamComponent
    {
        public Ironbug_HeatPumpPlantLoopEIRHeating()
          : base("IB_HeatPumpPlantLoopEIRHeating", "HtnHP",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatPumpPlantLoopEIRHeating_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CompanionCoolingHeatPump", "CompanionClnHP", "CompanionCoolingHeatPump", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatPumpPlantLoopEIRHeating", "HtnHP", "Connect to hot water loop's supply side.", GH_ParamAccess.item); 
            pManager[pManager.AddGenericParameter("HeatPumpPlantLoopEIRHeating_ToCondenser", "ToCondenser", "Connect to condenser loop's demand side.", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft;
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
            DA.SetDataList(1, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatPumpW2W_Heating;

        public override Guid ComponentGuid => new Guid("19D26BF9-93BE-4939-9D6F-049A6B5222E7");
    }
}