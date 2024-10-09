using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CentralHeatPumpSystem : Ironbug_HVACWithParamComponent
    {
        
        public Ironbug_CentralHeatPumpSystem()
          : base("IB_CentralHeatPumpSystem", "CentralHeatPumpSystem",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CentralHeatPumpSystem_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller-heaters", "chillerHeaters", "use ChillerHeaterPerformanceElectricEIR or CentralHeatPumpSystemModules. Typically three chillerheaters are needed", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToCW_Demand", "connect to condenser water plantloop's demand side", GH_ParamAccess.item);
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToCHW_Supply", "connect to chilled water plantloop's supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToHW_Supply", "connect to hot water plantloop's supply side", GH_ParamAccess.item);
            
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var chillersOrChillerModules  = new List<HVAC.BaseClass.IB_ModelObject>();
            DA.GetDataList(0, chillersOrChillerModules);



            var chillers = chillersOrChillerModules.OfType<HVAC.IB_ChillerHeaterPerformanceElectricEIR>();
            var modules = chillersOrChillerModules.OfType<HVAC.IB_CentralHeatPumpSystemModule>();

            var obj = new HVAC.IB_CentralHeatPumpSystem();
            //Add chillers
            foreach (var item in chillers)
            {
                var module = new HVAC.IB_CentralHeatPumpSystemModule();
                module.SetChillerHeater(item);
                //module.SetNumberOfChillerHeaterModules(chillers.Count);
                obj.AddModule(module);
            }

            //Add modules
            foreach (var item in modules)
            {
                obj.AddModule(item);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
            DA.SetData(2, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CentralHeatPump;//return null;

        
        public override Guid ComponentGuid => new Guid("32C6C154-746A-4B3E-A705-F6292C40476C");
    }
}