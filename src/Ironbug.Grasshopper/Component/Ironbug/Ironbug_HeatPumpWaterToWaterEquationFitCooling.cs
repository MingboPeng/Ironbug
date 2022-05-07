using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatPumpWaterToWaterEquationFitCooling : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_HeatPumpWaterToWaterEquationFitCooling()
          : base("IB_HeatPumpWaterToWaterEquationFitCooling", "HeatPumpHeating",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatPumpWaterToWaterEquationFitCooling_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatPumpW2W_Cooling;
        public override Guid ComponentGuid => new Guid("{E8798B4D-179E-4AF4-8F83-D13A13676FAD}");
        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatPumpWaterToWaterEquationFitCooling", "HP", "HeatPumpWaterToWaterEquationFitCooling at plantloop's demand side.", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatPumpWaterToWaterEquationFitCooling", "toSupplySide", "HeatPumpWaterToWaterEquationFitCooling at plantloop's supply side.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatPumpWaterToWaterEquationFitCooling();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }



    }

   
}