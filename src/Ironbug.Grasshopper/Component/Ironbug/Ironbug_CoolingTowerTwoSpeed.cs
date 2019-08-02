using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoolingTowerTwoSpeed : Ironbug_LoopObjectComponent
    {
        public Ironbug_CoolingTowerTwoSpeed()
          : base("Ironbug_CoolingTowerTwoSpeed", "CoolingTower2",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_CoolingTowerTwoSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingTowerTwoSpeed", "ClnTower", "CoolingTowerTwoSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoolingTowerTwoSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoolingTower2;

        public override Guid ComponentGuid => new Guid("663A4D95-AC94-45A3-BCE5-C65E7E52579C");
    }
}