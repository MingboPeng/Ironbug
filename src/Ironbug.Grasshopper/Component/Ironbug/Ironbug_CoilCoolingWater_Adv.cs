using Grasshopper.Kernel;
using System;


namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingWater_Adv : Ironbug_LoopObjectComponent
    {
        
        /// Initializes a new instance of the Ironbug_CoilCoolingWater class.
        
        public Ironbug_CoilCoolingWater_Adv()
          : base("Ironbug_CoilCoolingWater_Adv", "CoilClnWater_Adv", "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilCoolingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControllerWaterCoil", "_ctrl", "add a customized controller here", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirSide_CoilCoolingWater", "Coil", "Connect to air loop's supply side or other water cooled system.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilCoolingWater", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_ControllerWaterCoil ctrl = null;
            DA.GetData(0, ref ctrl);
            var obj = new HVAC.IB_CoilCoolingWater(ctrl);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilCW_adv;

        public override Guid ComponentGuid => new Guid("371E6816-D142-4599-B691-5F649BD74119");
    }
}