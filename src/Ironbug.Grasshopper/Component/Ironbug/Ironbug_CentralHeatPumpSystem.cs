using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CentralHeatPumpSystem : Ironbug_HVACComponent
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_PumpConstantSpeed class.
        /// </summary>
        public Ironbug_CentralHeatPumpSystem()
          : base("Ironbug_CentralHeatPumpSystem", "CentralHeatPumpSystem",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CentralHeatPumpSystem_DataFields))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Chiller-heaters", "chillerHeater", "use ChillerHeaterPerformanceElectricEIR", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToCWSupply", "connect to chilled water plantloop's supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToHWSupply", "connect to hot water plantloop's supply side", GH_ParamAccess.item);
            pManager.AddGenericParameter("CentralHeatPumpSystem", "ToDWDemand", "connect to condenser water plantloop's demand side", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var chillers = new List<HVAC.IB_ChillerHeaterPerformanceElectricEIR>();
            DA.GetDataList(0, chillers);
            
            var obj = new HVAC.IB_CentralHeatPumpSystem();
            foreach (var item in chillers)
            {
                var module = new HVAC.IB_CentralHeatPumpSystemModule();
                module.SetChillerHeater(item);

                module.SetNumberOfChillerHeaterModules(chillers.Count);
                obj.AddModule(module);
            }

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
            DA.SetData(1, obj);
            DA.SetData(2, obj);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.CentralHeatPump;//return null;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("32C6C154-746A-4B3E-A705-F6292C40476C");
    }
}