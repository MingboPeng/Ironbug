using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWaterBaseboard : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilHeatingWaterBaseboard()
          : base("Ironbug_CoilHeatingWaterBaseboard", "CoilHtn_Baseboard",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilHeatingWaterBaseboard_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterBaseboard", "Coil", "connect to baseboard", GH_ParamAccess.item);
            pManager[ pManager.AddGenericParameter("WaterSide_CoilHeatingWater", "ToWaterLoop", "connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft; ;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWaterBaseboard();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }



        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHWBaseboard;

        public override Guid ComponentGuid => new Guid("E80D4D7A-F34C-4217-A87A-B34E3372F79F");

    }
    
}