using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACLowTempRadiantConstFlow : Ironbug_HVACComponent
    {
        
        public Ironbug_ZoneHVACLowTempRadiantConstFlow()
          : base("Ironbug_ZoneHVACLowTempRadiantConstFlow", "RadiantCF",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACLowTempRadiantConstFlow_DataFieldSet))
        {
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.RadiantC;

        public override Guid ComponentGuid => new Guid("EF691837-6BEA-46A6-97CC-E97C9574F8AA");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatingCoil", "_coilH", "Heating coil to provide heating source. Must be CoilHeatingLowTempRadiantConstFlow.", GH_ParamAccess.item);
            
            pManager.AddGenericParameter("CoolingCoil", "_coilC", "Cooling coil to provide cooling source. Must be CoilCoolingLowTempRadiantConstFlow.", GH_ParamAccess.item);

            pManager.AddNumberParameter("PipeLength", "PipeLen_", "PipeLength", GH_ParamAccess.item, 200.0);
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACLowTempRadiantConstFlow", "RadiantCF", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var coilH = (IB_CoilHeatingLowTempRadiantConstFlow)null;
            var coilC = (IB_CoilCoolingLowTempRadiantConstFlow)null;
            var tubingLenght = 200.0;

            if (!DA.GetData(0, ref coilH)) return;
            if (!DA.GetData(1, ref coilC)) return;
            DA.GetData(2, ref tubingLenght);
            
            var obj = new HVAC.IB_ZoneHVACLowTempRadiantConstFlow(coilH,coilC, tubingLenght);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        
    }
}