using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilHeatingWater class.
        
        public Ironbug_CoilHeatingWater()
          : base("IB_CoilHeatingWater", "CoilHtnWater",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirSide_CoilHeatingWater", "Coil", "connect to air loop's supply side or other water heated system.", GH_ParamAccess.item);
            pManager[pManager.AddGenericParameter("WaterSide_CoilHeatingWater", "ToWaterLoop", "connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft;
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingWater();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHW;

        public override Guid ComponentGuid => new Guid("4f849460-bb38-441c-9387-95c5be5830e7");

    }
    
}