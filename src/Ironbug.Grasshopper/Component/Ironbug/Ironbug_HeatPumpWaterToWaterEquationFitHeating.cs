using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatPumpWaterToWaterEquationFitHeating : Ironbug_LoopObjectComponent
    {
        public Ironbug_HeatPumpWaterToWaterEquationFitHeating()
          : base("Ironbug_HeatPumpWaterToWaterEquationFitHeating", "HeatPumpHeating",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatPumpWaterToWaterEquationFitHeating_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HeatPumpW2W_Heating;
        public override Guid ComponentGuid => new Guid("{1335EF58-561F-40DC-B481-028C75E16F1D}");
        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatPumpWaterToWaterEquationFitHeating", "HP", "HeatPumpWaterToWaterEquationFitHeating", GH_ParamAccess.item);
            pManager.AddGenericParameter("HeatPumpWaterToWaterEquationFitHeating", "toSupplySide", "HeatPumpWaterToWaterEquationFitHeating", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatPumpWaterToWaterEquationFitHeating();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }



    }

   
}