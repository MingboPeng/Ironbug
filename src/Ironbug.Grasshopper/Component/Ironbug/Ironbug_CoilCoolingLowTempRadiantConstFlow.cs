using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingLowTempRadiantConstFlow : GH_Component
    {
        public Ironbug_CoilCoolingLowTempRadiantConstFlow()
          : base("Ironbug_CoilCoolingLowTempRadiantConstFlow", "CoilCW_LTRC",
              "Description",
              "Ironbug", "02:LoopComponents")
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_RC;

        public override Guid ComponentGuid => new Guid("A12E6626-3CD3-4A38-8944-99D5D45C7247");

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("High Water Temperature", "waterHiT", "High Water Temperature", GH_ParamAccess.item,15);
            pManager.AddNumberParameter("Low Water Temperature", "waterLoT", "Low Water Temperature", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("High Air Temperature", "airHiT", "High Air Temperature", GH_ParamAccess.item, 25);
            pManager.AddNumberParameter("Low Air Temperature", "airLoT", "Low Air Temperature", GH_ParamAccess.item, 21);

        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingLowTempRadiantConstFlow", "CoilCW_LTRC", "Add to ZoneHVACLowTempRadiantConstFlow", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingLowTempRadiantConstFlow", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double waterHiT = 15; //59F
            double waterLoT = 10; //50F
            double airHiT = 25; //77F
            double airLoT = 21; //70F

            DA.GetData(0, ref waterHiT);
            DA.GetData(1, ref waterLoT);
            DA.GetData(2, ref airHiT);
            DA.GetData(3, ref airLoT);

            var obj = new HVAC.IB_CoilCoolingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);
            
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }
        

    }
    
}