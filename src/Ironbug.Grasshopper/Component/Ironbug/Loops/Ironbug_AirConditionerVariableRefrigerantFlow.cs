using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_AirConditionerVariableRefrigerantFlow : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_AirConditionerVariableRefrigerantFlow()
          : base("Ironbug_AirConditionerVariableRefrigerantFlow", "VRFSystem",
              "Description",
              "Ironbug", "01:Loops",
              typeof(HVAC.IB_AirConditionerVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("terminals", "_terminals", "VRF terminals.", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirConditionerVariableRefrigerantFlow", "VRFSystem", "VRFSystem", GH_ParamAccess.item);
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
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.VRF;

        public override Guid ComponentGuid => new Guid("20B79F9A-1E3F-4A26-9D5B-5D6C988C15E4");
    }
}