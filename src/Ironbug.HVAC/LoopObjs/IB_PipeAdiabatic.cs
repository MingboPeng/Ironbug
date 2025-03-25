using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PipeAdiabatic : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PipeAdiabatic();
        private static PipeAdiabatic NewDefaultOpsObj(Model model)
            => new PipeAdiabatic(model);
        

        public IB_PipeAdiabatic():base(NewDefaultOpsObj)
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }

    public sealed class IB_PipeAdiabatic_FieldSet
        : IB_FieldSet<IB_PipeAdiabatic_FieldSet, PipeAdiabatic>
    {
        private IB_PipeAdiabatic_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

    }
}
