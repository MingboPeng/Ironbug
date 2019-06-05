using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ChillerHeaterPerformanceElectricEIR : Ironbug_HVACComponent
    {
        
        public Ironbug_ChillerHeaterPerformanceElectricEIR()
          : base("Ironbug_ChillerHeaterPerformanceElectricEIR", "ChillerHeater",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_ChillerHeaterPerformanceElectricEIR_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller-heaters", "chillerHeater", "connect to CentralHeatPumpSystem", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
           
            var obj = new HVAC.IB_ChillerHeaterPerformanceElectricEIR();
            var objs = this.SetObjParamsTo(obj);
            
            DA.SetDataList(0, objs);
        }

        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ChillerHeater;

        public override Guid ComponentGuid => new Guid("A5098132-CD94-465B-A8C9-D191E449E5C9");
    }
}