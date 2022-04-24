using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantComponentUserDefined : Ironbug_HVACWithParamComponent
    {
        public Ironbug_PlantComponentUserDefined()
          : base("Ironbug_PlantComponentUserDefined", "PlantComponent_UserDefined",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PlantComponentUserDefined_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_designVolumeFlowRateActuator", "_designVolumeFlowRateActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_maximumLoadingCapacityActuator", "_maximumLoadingCapacityActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_maximumMassFlowRateActuator", "_maximumMassFlowRateActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_minimumLoadingCapacityActuator", "_minimumLoadingCapacityActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_minimumMassFlowRateActuator", "_minimumMassFlowRateActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_optimalLoadingCapacityActuator", "_optimalLoadingCapacityActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_plantInitProgram", "_plantInitProgram", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_plantInitProgramCallingManager", "_plantInitProgramCallingManager", "", GH_ParamAccess.item);

            pManager.AddGenericParameter("_outletTemperatureActuator", "_outletTemperatureActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_massFlowRateActuator", "_massFlowRateActuator", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_plantSimulationProgram", "_plantSimulationProgram", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("_plantSimulationProgramCallingManager", "_plantSimulationProgramCallingManager", "", GH_ParamAccess.item);
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("plantComponentUserDefined", "plantComponent", "PlantComponentUserDefined for plant loop's supply.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new IB_PlantComponentUserDefined();

            IB_EnergyManagementSystemActuator act1 = null;
            DA.GetData(0, ref act1);
            obj.SetDesignVolumeFlowRateActuator(act1);

            IB_EnergyManagementSystemActuator act2 = null;
            DA.GetData(1, ref act2);
            obj.SetMaximumLoadingCapacityActuator(act2);

            IB_EnergyManagementSystemActuator act3 = null;
            DA.GetData(2, ref act3);
            obj.SetMaximumMassFlowRateActuator(act3);

            IB_EnergyManagementSystemActuator act4 = null;
            DA.GetData(3, ref act4);
            obj.SetMinimumLoadingCapacityActuator(act4);

            IB_EnergyManagementSystemActuator act5 = null;
            DA.GetData(4, ref act5);
            obj.SetMinimumMassFlowRateActuator(act5);

            IB_EnergyManagementSystemActuator act6 = null;
            DA.GetData(5, ref act6);
            obj.SetOptimalLoadingCapacityActuator(act6);


            IB_EnergyManagementSystemProgram program1 = null;
            DA.GetData(6, ref program1);
            obj.SetPlantInitializationProgram(program1);

            IB_EnergyManagementSystemProgramCallingManager manager1 = null;
            DA.GetData(7, ref manager1);
            obj.SetPlantInitializationProgramCallingManager(manager1);


            IB_EnergyManagementSystemActuator act7 = null;
            DA.GetData(8, ref act7);
            obj.SetOutletTemperatureActuator(act7);

            IB_EnergyManagementSystemActuator act8 = null;
            DA.GetData(9, ref act8);
            obj.SetMassFlowRateActuator(act8);

            IB_EnergyManagementSystemProgram program2 = null;
            DA.GetData(10, ref program2);
            obj.SetPlantSimulationProgram(program2);

            IB_EnergyManagementSystemProgramCallingManager manager2 = null;
            DA.GetData(11, ref manager2);
            obj.SetPlantSimulationProgramCallingManager(manager2);

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.PlantComponent;

        public override Guid ComponentGuid => new Guid("B85DA677-8B01-47BB-B061-8106B6D228DF");
    }
}