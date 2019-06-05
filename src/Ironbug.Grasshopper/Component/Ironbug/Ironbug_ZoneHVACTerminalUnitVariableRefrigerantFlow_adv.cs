using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Adv : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow class.
        
        public Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Adv()
          : base("Ironbug_ZoneHVACTerminalUnitVariableRefrigerantFlow_Advanced", "VRFTerminal_Adv",
              "Use this component to set customized cooling coil, heating coil, or fan for VRF terminal unit.",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager[pManager.AddGenericParameter("CoilCoolingDXVariableRefrigerantFlow", "ClnCoil",
                "Optional input, but use CoilCoolingDXVariableRefrigerantFlow only", GH_ParamAccess.item)].Optional = true;

            pManager[pManager.AddGenericParameter("CoilHeatingDXVariableRefrigerantFlow", "HtnCoil",
                "Optional input, but use CoilHeatingDXVariableRefrigerantFlow only", GH_ParamAccess.item)].Optional = true;

            pManager[pManager.AddGenericParameter("FanOnOff", "Fan",
                "Optional input, but use FanOnOff only", GH_ParamAccess.item)].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "VRFUnit", "Connect to Zone's equipment", GH_ParamAccess.item);
            pManager.AddGenericParameter("ZoneHVACTerminalUnitVariableRefrigerantFlow", "ToOutdoorUnit", "Connect to VRF system", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_CoilCoolingDXVariableRefrigerantFlow cCoil= null;
            IB_CoilHeatingDXVariableRefrigerantFlow hCoil = null;
            IB_FanOnOff fan = null;

            if (!DA.GetData(0, ref cCoil)) cCoil = new IB_CoilCoolingDXVariableRefrigerantFlow();
            if (!DA.GetData(1, ref hCoil)) hCoil = new IB_CoilHeatingDXVariableRefrigerantFlow();
            if (!DA.GetData(2, ref fan)) fan = new IB_FanOnOff();

            var obj = new HVAC.IB_ZoneHVACTerminalUnitVariableRefrigerantFlow(cCoil, hCoil, fan);
            
            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.VRFUnit_adv;

        public override Guid ComponentGuid => new Guid("{6DF5E370-7A09-4CB7-9A75-AA1D822346E9}");
    }
}