using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDXVariableRefrigerantFlow : Ironbug_HVACComponent
    {

        public Ironbug_CoilHeatingDXVariableRefrigerantFlow()
          : base("Ironbug_CoilHeatingDXVariableRefrigerantFlow", "CoilHtn_VRF",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilHeatingDXVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXVariableRefrigerantFlow", "Coil", "Connect to Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Advanced", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingDXVariableRefrigerantFlow();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_HeatingVRF;

        public override Guid ComponentGuid => new Guid("{B03CBA20-A469-4443-810A-DF32F615338D}");

    }
    
}