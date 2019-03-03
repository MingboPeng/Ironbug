using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACLowTempRadiantVarFlow : Ironbug_HVACComponent
    {
        
        public Ironbug_ZoneHVACLowTempRadiantVarFlow()
          : base("Ironbug_ZoneHVACLowTempRadiantVarFlow", "RadiantVFlow",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACLowTempRadiantVarFlow_DataFieldSet))
        {
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.RadiantV;

        public override Guid ComponentGuid => new Guid("8F88101E-1B7A-44AF-98D5-ED8B5AC861F5");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "_coilH", "Heating coil to provide heating source. Must be CoilHeatingLowTempRadiantVarFlow.", GH_ParamAccess.item);
            
            pManager.AddGenericParameter("CoolingCoil", "_coilC", "Cooling coil to provide cooling source. Must be CoilCoolingLowTempRadiantVarFlow.", GH_ParamAccess.item);
            
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACLowTempRadiantVarFlow", "RadiantVF", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var coilH = (IB_CoilHeatingLowTempRadiantVarFlow)null;
            var coilC = (IB_CoilCoolingLowTempRadiantVarFlow)null; 

            if (!DA.GetData(0, ref coilH)) return;
            if (!DA.GetData(1, ref coilC)) return; 
            
            var obj = new HVAC.IB_ZoneHVACLowTempRadiantVarFlow(coilH,coilC);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        
    }
}