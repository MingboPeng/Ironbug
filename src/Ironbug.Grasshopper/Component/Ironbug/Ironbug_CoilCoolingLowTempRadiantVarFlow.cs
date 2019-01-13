using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingLowTempRadiantVarFlow : Ironbug_HVACComponentBase
    {
        public Ironbug_CoilCoolingLowTempRadiantVarFlow()
          : base("Ironbug_CoilCoolingLowTempRadiantVarFlow", "CoilCW_LTRV",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingLowTempRadiantVarFlow_DataFieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_RV;

        public override Guid ComponentGuid => new Guid("69FD93C0-2DCA-4F61-BF41-7C43C1044101");

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("High Air Temperature", "airHiT", "High control air temperature, below which the cooling will be turned on", GH_ParamAccess.item, 50);

        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingLowTempRadiantVarFlow", "CoilCW_LTRV", "Add to ZoneHVACLowTempRadiantVarFlow", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingLowTempRadiantVarFlow", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double airHiT = 50;
            
            DA.GetData(0, ref airHiT);

            var obj = new HVAC.IB_CoilCoolingLowTempRadiantVarFlow( airHiT);
            obj.PuppetEventHandler += PuppetStateChanged;
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }
        

    }
    
}