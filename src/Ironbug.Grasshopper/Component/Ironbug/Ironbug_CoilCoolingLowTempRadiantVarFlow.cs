using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingLowTempRadiantVarFlow : Ironbug_HVACComponent
    {
        public Ironbug_CoilCoolingLowTempRadiantVarFlow()
          : base("Ironbug_CoilCoolingLowTempRadiantVarFlow", "CoilCln_LowTRadV",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilCoolingLowTempRadiantVarFlow_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_RV;

        public override Guid ComponentGuid => new Guid("69FD93C0-2DCA-4F61-BF41-7C43C1044101");

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("High Air Temperature", "airHiT", "High control air temperature, above which the cooling will be turned on", GH_ParamAccess.item, 24);

        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingLowTempRadiantVarFlow", "Coil", "Add to ZoneHVACLowTempRadiantVarFlow", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingLowTempRadiantVarFlow", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double airHiT = 24;
            
            DA.GetData(0, ref airHiT);

            var obj = new HVAC.IB_CoilCoolingLowTempRadiantVarFlow( airHiT);
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }
        

    }
    
}