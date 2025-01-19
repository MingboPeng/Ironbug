using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ChillerAbsorptionIndirect : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ChillerAbsorptionIndirect();



        private static ChillerAbsorptionIndirect NewDefaultOpsObj(Model model) => new ChillerAbsorptionIndirect(model);

        public IB_ChillerAbsorptionIndirect() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }



    public sealed class IB_ChillerAbsorptionIndirect_FieldSet
        : IB_FieldSet<IB_ChillerAbsorptionIndirect_FieldSet, ChillerAbsorptionIndirect>
    {
        private IB_ChillerAbsorptionIndirect_FieldSet() { }

    }
}
