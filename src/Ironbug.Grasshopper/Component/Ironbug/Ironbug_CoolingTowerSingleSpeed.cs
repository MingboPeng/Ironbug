using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoolingTowerSingleSpeed : Ironbug_LoopObjectComponent
    {
        
        
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        
        public Ironbug_CoolingTowerSingleSpeed()
          : base("Ironbug_CoolingTowerSingleSpeed", "CoolingTower1",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_CoolingTowerSingleSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingTowerSingleSpeed", "ClnTower", "CoolingTowerSingleSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoolingTowerSingleSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoolingTower1;

        public override Guid ComponentGuid => new Guid("40943146-68A0-4AFA-9CE9-8FC74931A8F0");
    }
}