using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EnergyManagementSystem : Ironbug_Component
    {
        public Ironbug_EnergyManagementSystem()
          : base("Ironbug_EMS", "EMS",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("actuators", "actuators", "Actuators", GH_ParamAccess.list);
            pManager.AddGenericParameter("sensors", "sensors", "Sensors", GH_ParamAccess.list);
            pManager.AddGenericParameter("variables", "variables", "Variables", GH_ParamAccess.list);
            pManager.AddGenericParameter("programCallingManagers", "managers", "ProgramCallingManagers", GH_ParamAccess.list);

            pManager[0].Optional = true;
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[1].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
            pManager[2].Optional = true;
            pManager[2].DataMapping = GH_DataMapping.Flatten;
            pManager[3].Optional = true;
            pManager[3].DataMapping = GH_DataMapping.Flatten;

        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EnergyManagementSystem", "EMS", ".", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var actuators = new List<HVAC.IB_EnergyManagementSystemActuator>();
            var sensors = new List<HVAC.IB_EnergyManagementSystemSensor>();
            var variables = new List<HVAC.BaseClass.IB_EnergyManagementSystemVariable>();
            var prgmManagers = new List<HVAC.IB_EnergyManagementSystemProgramCallingManager>();

            DA.GetDataList(0, actuators);
            DA.GetDataList(1, sensors);
            DA.GetDataList(2, variables);
            DA.GetDataList(3, prgmManagers);

            var hvac = new HVAC.IB_EnergyManagementSystem(actuators, sensors, variables, prgmManagers);
            DA.SetData(0, hvac);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HVACEMS;

        public override Guid ComponentGuid => new Guid("B3E08C2C-43EF-4046-8C23-B57E573203EA");


    }
}