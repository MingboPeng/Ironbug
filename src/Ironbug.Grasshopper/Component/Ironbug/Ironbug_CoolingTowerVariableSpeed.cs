using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoolingTowerVariableSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoolingTowerVariableSpeed()
          : base("Ironbug_CoolingTowerVariableSpeed", "CoolingTowerV",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_CoolingTowerVariableSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoolingTowerVariableSpeed", "ClnTower", "CoolingTowerVariableSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoolingTowerVariableSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoolingTowerV;

        public override Guid ComponentGuid => new Guid("D56FCFB6-02A8-4E39-804A-5B0DE9ED6E56");
    }
}