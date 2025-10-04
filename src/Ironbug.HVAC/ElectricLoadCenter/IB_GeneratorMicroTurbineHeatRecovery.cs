//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Ironbug.HVAC.BaseClass;
//using OpenStudio;

//namespace Ironbug.HVAC
//{
//    public class IB_GeneratorMicroTurbineHeatRecovery : IB_Generator
//    {
//        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorMicroTurbineHeatRecovery();

//        private static GeneratorMicroTurbineHeatRecovery NewDefaultOpsObj(Model model) => new(model, new GeneratorMicroTurbine(model));
//        private IB_GeneratorMicroTurbine _microTurbine => this.GetChild<IB_GeneratorMicroTurbine>(0);

//        public IB_GeneratorMicroTurbineHeatRecovery() : base(NewDefaultOpsObj)
//        {
//        }

//        public override Generator ToOS(Model model)
//        {
//            var turbine = _microTurbine.ToOS(model) as GeneratorMicroTurbine;
//            var obj = base.OnNewOpsObj((model) =>  new GeneratorMicroTurbineHeatRecovery(model, turbine), model);
//            return obj;
//        }

//    }

//    public sealed class IB_GeneratorMicroTurbineHeatRecovery_FieldSet
//       : IB_FieldSet<IB_GeneratorMicroTurbineHeatRecovery_FieldSet, GeneratorMicroTurbineHeatRecovery>
//    {

//        private IB_GeneratorMicroTurbineHeatRecovery_FieldSet() { }

//    }


//}
