using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXVariableRefrigerantFlow : Ironbug_HVACComponent
    {

        public Ironbug_CoilCoolingDXVariableRefrigerantFlow()
          : base("Ironbug_CoilCoolingDXVariableRefrigerantFlow", "CoilCln_VRF",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilCoolingDXVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXVariableRefrigerantFlow", "Coil", "Connect to Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Advanced", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXVariableRefrigerantFlow();
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_CoolingVRF;

        public override Guid ComponentGuid => new Guid("{57BC010A-EA48-42C3-9C92-333EC2C1DD07}");

    }
    
}