
using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CentralHeatPumpSystemModule : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CentralHeatPumpSystemModule();

        private static CentralHeatPumpSystemModule NewDefaultOpsObj(Model model) => new CentralHeatPumpSystemModule(model);

        private IB_ChillerHeaterPerformanceElectricEIR _chiller => this.GetChild<IB_ChillerHeaterPerformanceElectricEIR>();

        public int NumberOfChillerHeaterModules { get; private set; }

        public IB_CentralHeatPumpSystemModule() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_ChillerHeaterPerformanceElectricEIR());
        }

        public void SetChillerHeater(IB_ChillerHeaterPerformanceElectricEIR Chiller)
        {
            this.SetChild(Chiller);
        }

        public void SetNumberOfChillerHeaterModules(int Number)
        {
            this.NumberOfChillerHeaterModules = Number;
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = base.Duplicate() as  IB_CentralHeatPumpSystemModule;
            newObj.SetNumberOfChillerHeaterModules(this.NumberOfChillerHeaterModules);

            return newObj;
        }


        public ModelObject ToOS(Model model)
        {
            var newObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            newObj.chillerHeaterModulesPerformanceComponent().remove();

            var chillerHeater = this._chiller.ToOS(model) as ChillerHeaterPerformanceElectricEIR;
            newObj.setChillerHeaterModulesPerformanceComponent(chillerHeater);
            newObj.setNumberofChillerHeaterModules(this.NumberOfChillerHeaterModules);
            var count = model.getChillerHeaterPerformanceElectricEIRs().Count;
          
            return newObj;
        }
    }

    public sealed class IB_CentralHeatPumpSystemModule_FieldSet
      : IB_FieldSet<IB_CentralHeatPumpSystemModule_FieldSet, CentralHeatPumpSystemModule>
    {
        private IB_CentralHeatPumpSystemModule_FieldSet() { }

    }


}