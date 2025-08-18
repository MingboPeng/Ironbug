//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Ironbug.HVAC.BaseClass;
//using OpenStudio;

//namespace Ironbug.HVAC
//{
//    public class IB_GeneratorMicroTurbineHeatRecovery : IB_ModelObject
//    {
//        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorMicroTurbineHeatRecovery();

//        private static GeneratorMicroTurbineHeatRecovery NewDefaultOpsObj(Model model) => new(model);

//        public IB_GeneratorMicroTurbineHeatRecovery() : base(NewDefaultOpsObj)
//        {
//        }
       
//        public GeneratorMicroTurbineHeatRecovery ToOS(Model model)
//        {
//            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
//            return obj;
//        }

//    }

//    public sealed class IB_GeneratorMicroTurbineHeatRecovery_FieldSet
//       : IB_FieldSet<IB_GeneratorMicroTurbineHeatRecovery_FieldSet, GeneratorMicroTurbineHeatRecovery>
//    {

//        private IB_GeneratorMicroTurbineHeatRecovery_FieldSet() { }

//    }


//}
