using Grasshopper.Kernel;
using System;


namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingWater : Ironbug_DuplicableHVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilCoolingWater class.
        
        public Ironbug_CoilCoolingWater()
          : base("Ironbug_CoilCoolingWater", "CoilClnWater","",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirSide_CoilCoolingWater", "Coil", "Connect to air loop's supply side or other water cooled system.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingWater", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingWater();
            

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCW;

        
        public override Guid ComponentGuid => new Guid("42c0bccb-cb71-40af-83cf-14fa9a01f3ea");
    }
}