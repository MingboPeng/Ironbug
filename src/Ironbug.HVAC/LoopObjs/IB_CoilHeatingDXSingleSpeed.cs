using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXSingleSpeed: IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXSingleSpeed();
        private static CoilHeatingDXSingleSpeed NewDefaultOpsObj(Model model) => new CoilHeatingDXSingleSpeed(model);

        public IB_CoilHeatingDXSingleSpeed() : base(NewDefaultOpsObj(new Model()))
        {

        }
        


        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilHeatingDXSingleSpeed_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingDXSingleSpeed_DataFieldSet, CoilHeatingDXSingleSpeed>
    {
        private IB_CoilHeatingDXSingleSpeed_DataFieldSet() { }

    }
}
