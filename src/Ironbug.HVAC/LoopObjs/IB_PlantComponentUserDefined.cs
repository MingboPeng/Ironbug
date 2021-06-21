using System;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_PlantComponentUserDefined : IB_HVACObject,  IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantComponentUserDefined();

        private static PlantComponentUserDefined NewDefaultOpsObj(Model model) => new PlantComponentUserDefined(model);
 
        private IB_EnergyManagementSystemActuator _dvfrActuator { get; set; }
        private IB_EnergyManagementSystemActuator _mmfrActuator { get; set; }
        private IB_EnergyManagementSystemActuator _mxfrActuator { get; set; }
        private IB_EnergyManagementSystemActuator _mlcActuator { get; set; }
        private IB_EnergyManagementSystemActuator _mxlcActuator { get; set; }
        private IB_EnergyManagementSystemActuator _olcActuator { get; set; }
        private IB_EnergyManagementSystemActuator _otActuator { get; set; }
        private IB_EnergyManagementSystemActuator _mfrActuator { get; set; }

        private IB_EnergyManagementSystemProgram _plantSimulationProgram { get; set; }
        private IB_EnergyManagementSystemProgramCallingManager _plantSimulationProgramCallingManager { get; set; }
        private IB_EnergyManagementSystemProgram _plantInitializationProgram { get; set; }
        private IB_EnergyManagementSystemProgramCallingManager _plantInitializationProgramCallingManager { get; set; }

        public IB_PlantComponentUserDefined() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetDesignVolumeFlowRateActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._dvfrActuator = energyManagementSystemActuator; }
        public void SetMassFlowRateActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._mfrActuator = energyManagementSystemActuator; }
        public void SetMaximumLoadingCapacityActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._mxlcActuator = energyManagementSystemActuator; }
        public void SetMaximumMassFlowRateActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._mxfrActuator = energyManagementSystemActuator; }
        public void SetMinimumLoadingCapacityActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._mlcActuator = energyManagementSystemActuator; }
        public void SetMinimumMassFlowRateActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._mmfrActuator = energyManagementSystemActuator; }
        public void SetOptimalLoadingCapacityActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._olcActuator = energyManagementSystemActuator; }
        public void SetOutletTemperatureActuator(IB_EnergyManagementSystemActuator energyManagementSystemActuator) { this._otActuator = energyManagementSystemActuator; }

        public void SetPlantInitializationProgram(IB_EnergyManagementSystemProgram program) { this._plantInitializationProgram = program; }
        public void SetPlantInitializationProgramCallingManager(IB_EnergyManagementSystemProgramCallingManager manager) { this._plantInitializationProgramCallingManager = manager; }


        public void SetPlantSimulationProgram(IB_EnergyManagementSystemProgram program) { this._plantSimulationProgram = program; }
        public void SetPlantSimulationProgramCallingManager(IB_EnergyManagementSystemProgramCallingManager manager) { this._plantSimulationProgramCallingManager = manager; }
        public override IB_ModelObject Duplicate()
        {
            var dup = base.Duplicate() as IB_PlantComponentUserDefined;
            dup._dvfrActuator = this._dvfrActuator;
            dup._mfrActuator = this._mfrActuator;
            dup._mxlcActuator = this._mxlcActuator;
            dup._mxfrActuator = this._mxfrActuator;
            dup._mlcActuator = this._mlcActuator;
            dup._mmfrActuator = this._mmfrActuator;
            dup._olcActuator = this._olcActuator;
            dup._otActuator = this._otActuator;

            dup._plantInitializationProgram = this._plantInitializationProgram;
            dup._plantInitializationProgramCallingManager = this._plantInitializationProgramCallingManager;

            dup._plantSimulationProgram = this._plantSimulationProgram;
            dup._plantSimulationProgramCallingManager = this._plantSimulationProgramCallingManager;
            return dup;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            var mapper = new Dictionary<IB_ModelObject, ModelObject>();

            var dvfr = obj.designVolumeFlowRateActuator().get();
            _dvfrActuator.ApplyAttributesToObj(dvfr);
            mapper.Add(_dvfrActuator, dvfr);

            var mfr = obj.massFlowRateActuator().get();
            _mfrActuator.ApplyAttributesToObj(mfr);
            mapper.Add(_mfrActuator, mfr);

            var mxlc = obj.maximumLoadingCapacityActuator().get();
            _mxlcActuator.ApplyAttributesToObj(mxlc);
            mapper.Add(_mxlcActuator, mxlc);

            var mxfr = obj.maximumMassFlowRateActuator().get();
            _mxfrActuator.ApplyAttributesToObj(mxfr);
            mapper.Add(_mxfrActuator, mxfr);


            var mlc = obj.minimumLoadingCapacityActuator().get();
            _mlcActuator.ApplyAttributesToObj(mlc);
            mapper.Add(_mlcActuator, mlc);

            var mmfr = obj.minimumMassFlowRateActuator().get();
            _mmfrActuator.ApplyAttributesToObj(mmfr);
            mapper.Add(_mmfrActuator, mmfr);

            var olc = obj.optimalLoadingCapacityActuator().get();
            _olcActuator.ApplyAttributesToObj(olc);
            mapper.Add(_olcActuator, olc);

            var ot = obj.outletTemperatureActuator().get();
            _otActuator.ApplyAttributesToObj(ot);
            mapper.Add(_otActuator, ot);

            var idMapper = mapper.ToDictionary(_ =>  _.Key.GetTrackingID(), v=> v.Value.handle().__str__());

            var psp = obj.plantSimulationProgram().get();
            _plantSimulationProgram.ApplyAttributesToObj(psp, idMapper);

            var pspm = obj.plantSimulationProgramCallingManager().get();
            _plantSimulationProgramCallingManager.ApplyAttributesToObj(pspm);

            var pip = obj.plantInitializationProgram().get();
            _plantInitializationProgram.ApplyAttributesToObj(pip, idMapper);

            var pipm = obj.plantInitializationProgramCallingManager().get();
            _plantInitializationProgramCallingManager.ApplyAttributesToObj(pipm);


            return obj;

        }

        #region ops helpers


        //public EnergyManagementSystemProgram createInitProgram(EnergyManagementSystemActuator dvfrActuator, EnergyManagementSystemActuator mmfrActuator,
        //EnergyManagementSystemActuator mxfrActuator, EnergyManagementSystemActuator mlcActuator, EnergyManagementSystemActuator mxlcActuator, EnergyManagementSystemActuator olcActuator, Model model);

        //public EnergyManagementSystemProgram createSimProgram(EnergyManagementSystemActuator otActuator, EnergyManagementSystemActuator mfrActuator, Model model);

        //public bool setMainModelProgram(EnergyManagementSystemProgram energyManagementSystemProgram);
        //public bool setMainModelProgramCallingManager(EnergyManagementSystemProgramCallingManager energyManagementSystemProgramCallingManager);


        //public bool setDesignVolumeFlowRateActuator(EnergyManagementSystemActuator energyManagementSystemActuator);    dvfrActuator
        //public bool setMassFlowRateActuator(EnergyManagementSystemActuator energyManagementSystemActuator);            mfrActuator
        //public bool setMaximumLoadingCapacityActuator(EnergyManagementSystemActuator energyManagementSystemActuator);  mxlcActuator 
        //public bool setMaximumMassFlowRateActuator(EnergyManagementSystemActuator energyManagementSystemActuator);     mxfrActuator
        //public bool setMinimumLoadingCapacityActuator(EnergyManagementSystemActuator energyManagementSystemActuator);  mlcActuator
        //public bool setMinimumMassFlowRateActuator(EnergyManagementSystemActuator energyManagementSystemActuator);     mmfrActuator
        //public bool setOptimalLoadingCapacityActuator(EnergyManagementSystemActuator energyManagementSystemActuator);  olcActuator
        //public bool setOutletTemperatureActuator(EnergyManagementSystemActuator energyManagementSystemActuator);       otActuator

        //public bool setPlantInitializationProgram(EnergyManagementSystemProgram energyManagementSystemProgram);
        //public bool setPlantInitializationProgramCallingManager(EnergyManagementSystemProgramCallingManager energyManagementSystemProgramCallingManager);

        //public bool setPlantLoadingMode(string plantLoadingMode);
        //public bool setPlantLoopFlowRequestMode(string plantLoopFlowRequestMode);

        //public bool setPlantSimulationProgram(EnergyManagementSystemProgram energyManagementSystemProgram);
        //public bool setPlantSimulationProgramCallingManager(EnergyManagementSystemProgramCallingManager energyManagementSystemProgramCallingManager);

        #endregion
    }

    public sealed class IB_PlantComponentUserDefined_FieldSet
        : IB_FieldSet<IB_PlantComponentUserDefined_FieldSet, PlantComponentUserDefined>
    {
        private IB_PlantComponentUserDefined_FieldSet() { }

    }



}
