using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CentralHeatPumpSystemModule : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CentralHeatPumpSystemModule()
          : base("Ironbug_CentralHeatPumpSystemModule", "ChillerHeaterModule",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CentralHeatPumpSystemModule_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller-heater", "chillerHeater", "use ChillerHeaterPerformanceElectricEIR", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller-heater Module", "chillerHeaterM", "connect to CentralHeatPumpSystem", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var chiller = new HVAC.IB_ChillerHeaterPerformanceElectricEIR();
            DA.GetData(0, ref chiller);

          
            var obj = new HVAC.IB_CentralHeatPumpSystemModule();
            obj.SetChillerHeater(chiller);

            this.SetObjParamsTo(obj);
            
            DA.SetData(0, obj);
        }

        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ChillerHeater;

        public override Guid ComponentGuid => new Guid("{60CCB14B-A70F-4164-90F7-02332B1BCA47}");
    }
}