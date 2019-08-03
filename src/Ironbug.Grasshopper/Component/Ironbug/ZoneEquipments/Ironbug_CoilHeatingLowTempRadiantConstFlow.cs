using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingLowTempRadiantConstFlow : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoilHeatingLowTempRadiantConstFlow()
          : base("Ironbug_CoilHeatingLowTempRadiantConstFlow", "CoilHtn_LowTRadC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilHeatingLowTempRadiantConstFlow_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHWRC;

        public override Guid ComponentGuid => new Guid("2B00C081-5C57-46F7-833A-845C55F11831");

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("High Water Temperature", "waterHiT", "High Water Temperature", GH_ParamAccess.item,50);
            pManager.AddNumberParameter("Low Water Temperature", "waterLoT", "Low Water Temperature", GH_ParamAccess.item, 30);
            pManager.AddNumberParameter("High Air Temperature", "airHiT", "High Air Temperature", GH_ParamAccess.item, 20);
            pManager.AddNumberParameter("Low Air Temperature", "airLoT", "Low Air Temperature", GH_ParamAccess.item, 17);

        }
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingLowTempRadiantConstFlow", "Coil", "Add to ZoneHVACLowTempRadiantConstFlow", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilHeatingLowTempRadiantConstFlow", "ToWaterLoop", "Connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double waterHiT = 50; //122F
            double waterLoT = 30; //86F
            double airHiT = 20; //68F
            double airLoT = 17; //62.6F

            DA.GetData(0, ref waterHiT);
            DA.GetData(1, ref waterLoT);
            DA.GetData(2, ref airHiT);
            DA.GetData(3, ref airLoT);

            var obj = new HVAC.IB_CoilHeatingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }
        

    }
    
}