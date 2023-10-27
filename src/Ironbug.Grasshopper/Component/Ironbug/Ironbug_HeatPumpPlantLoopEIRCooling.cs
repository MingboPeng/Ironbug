using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatPumpPlantLoopEIRCooling : Ironbug_HVACWithParamComponent
    {
        public Ironbug_HeatPumpPlantLoopEIRCooling()
          : base("IB_HeatPumpPlantLoopEIRCooling", "ClnHP",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatPumpPlantLoopEIRCooling_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CompanionHeatingHeatPump", "CompanionHtnHP", "CompanionHeatingHeatPump", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatPumpPlantLoopEIRCooling", "ClnHP", "Connect to chilled water loop's supply side.", GH_ParamAccess.item);
            pManager[pManager.AddGenericParameter("HeatPumpPlantLoopEIRCooling_ToCondenser", "ToCondenser", "Connect to condenser loop's demand side.", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft;
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_HeatPumpPlantLoopEIRHeating hp = null;
            var obj = new HVAC.IB_HeatPumpPlantLoopEIRCooling();
            if (DA.GetData(0, ref hp) && hp != null)
            {
                obj.SetCompanionHeatingHeatPump(hp);
            }

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatPumpW2W_Cooling;

        public override Guid ComponentGuid => new Guid("C75B7FF9-CD54-49A5-BBB7-228C64605DE4");
    }
}