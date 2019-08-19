using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingWater_Adv : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CoilHeatingWater_Adv()
          : base("Ironbug_CoilHeatingWater_Adv", "CoilHtnWater_Adv",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ControllerWaterCoil", "_ctrl", "add a customized controller here", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirSide_CoilHeatingWater", "Coil", "connect to air loop's supply side or other water heated system.", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide_CoilHeatingWater", "ToWaterLoop", "connect to hot water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_ControllerWaterCoil ctrl = null;
            DA.GetData(0, ref ctrl);
            var obj = new HVAC.IB_CoilHeatingWater(ctrl);
            

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHW_adv;

        public override Guid ComponentGuid => new Guid("0DE0F2D8-6A20-4A47-8C4B-BC149BA7E116");

    }
    
}