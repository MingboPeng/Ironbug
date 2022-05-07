using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirConditionerVariableRefrigerantFlow_WaterCooled : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_AirConditionerVariableRefrigerantFlow_WaterCooled()
          : base("IB_AirConditionerVariableRefrigerantFlow_WaterCooled", "VRFSystem_WaterCooled",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_AirConditionerVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("terminals", "_terminals", "VRF terminals.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CondenserPlantLoopBranch", "ToPlantBranch", "Connect to condenser plant loop's demand branch", GH_ParamAccess.item);
            pManager.AddGenericParameter("AirConditionerVariableRefrigerantFlow", "VRFSystem", "Connect to HVACSystem's VRFSystem input", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var obj = new HVAC.IB_AirConditionerVariableRefrigerantFlow();
            var terminals = new List<HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>();
            DA.GetDataList(0, terminals);
            foreach (var term in terminals)
            {
                obj.AddTerminal(term);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.VRF;

        public override Guid ComponentGuid => new Guid("{B91DA061-493B-4490-B86D-0E3F8E4EE6E0}");
    }
}