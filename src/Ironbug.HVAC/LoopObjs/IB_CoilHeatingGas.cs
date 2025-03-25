using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingGas : IB_CoilHeatingBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingGas();

        private static CoilHeatingGas NewDefaultOpsObj(Model model) => new CoilHeatingGas(model);

        public IB_CoilHeatingGas() : base(NewDefaultOpsObj)
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilHeatingGas_FieldSet
        : IB_FieldSet<IB_CoilHeatingGas_FieldSet, CoilHeatingGas>
    {
        
        //internal override Type RefEpType => typeof(EPDoc.CoilHeatingFuel);
        private IB_CoilHeatingGas_FieldSet() {
        }

    }
}
